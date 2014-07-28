using UnityEngine;
using System.Collections;

public class RoomManagerScript : MonoBehaviour {
	
	CameraScript leftCamera, rightCamera;
	PlayerControllerScript leftPlayer, rightPlayer;

	BoxCollider2D roomCollider, roomColliderPrev;
	private int roomLayerMasks;

	// Rect that defines the room (measured in tiles)
	Rect _roomRect;
	
	// "Helper" Properties
	public Vector2 roomCenter{
		get{ return _roomRect.center; }
	}
	public Vector2 roomSize{
		get{ return _roomRect.size; }
	}

	// Room bounds
	// roomRight refers to the farthest right tile that's inside the room
	public int roomTop{
		get{ return Mathf.RoundToInt(_roomRect.yMax); }
	}
	public int roomBot{
		get{ return Mathf.RoundToInt(_roomRect.yMin); }
	}
	public int roomLeft{
		get{ return Mathf.RoundToInt(_roomRect.xMin); }
	}
	public int roomRight{
		get{ return Mathf.RoundToInt(_roomRect.xMax); }
	}

	// Track how many cameras have finished their transition
	int cameraFinishes;

	void Start () {
		leftCamera = Globals.cameraLeft;
		rightCamera = Globals.cameraRight;

		leftPlayer = Globals.playerLeft;
		rightPlayer = Globals.playerRight;

		cameraFinishes = 0;

		// Standard 9 x 10 
		SetBounds (-4, -5, 4, 4); 
	}

	void Update() {
		//Check for only the left and right room layers
		roomLayerMasks = ( (1 << LayerMask.NameToLayer("RoomsRight")) | 
		                    (1 << LayerMask.NameToLayer("RoomsLeft")) );

		roomCollider = (BoxCollider2D) Physics2D.Raycast(new Vector2(leftPlayer.x, leftPlayer.y), 
		                                             Vector2.zero, 
		                                             0f, 
		                                             roomLayerMasks).collider;

		if (roomCollider != roomColliderPrev && roomCollider != null) {
			MoveScreen();
		}

		roomColliderPrev = roomCollider;
	}

	// Temprary hack to reset the camera when the player falls off the level
	public void Reset(){
		leftCamera.BeginRoomTransitionFade(CameraTransitionFinished);
		rightCamera.BeginRoomTransitionFade(CameraTransitionFinished);

		SetBounds (-4, -5, 4, 4);
	}

	public void RunTinyRoomTest(){SetBounds(-2, -2, 2, 2);}
	public void RunBigRoomTest(){SetBounds(-9, -10, 10, 10);}
	
	public bool ContainsTile(Vector2 tile){
		return _roomRect.Contains(tile);
	}

	void SetRoomRect(float left, float top, float width, float height){
		// Rect objects use a GUI coordinate system where the y axis is opposite than in 3D view
		// So we actually pass in (left, BOT, width, height) instead of (left, TOP...)
		_roomRect = new Rect(left, top-height, width, height);
	}

	/// <summary>
	/// Sets the bounds.
	/// </summary>
	/// <param name="left">Left, a.</param>
	/// <param name="bot">Bot, a.</param>
	/// <param name="right">Right, a.</param>
	/// <param name="top">Top, a.</param>
	void SetBounds(int left, int bot, int right, int top){
		SetRoomRect(left, top, right - left, top - bot);
	}

	void LogRoomInfo(){
		Debug.Log("Room Center = (" + roomCenter.x + ", " + roomCenter.y + 
		          ")   Size = " + roomSize.x + " x " + roomSize.y);
		Debug.Log ("Room Bounds: (" + roomLeft + ", " + roomTop + ") to (" + roomRight + ", " + roomBot + ")");
	}

	public void MoveScreen(){
		// Need to wait a short bit before transitioning 
		// because one character will get stuck if he hasn't moved yet
		CancelInvoke("BeginCameraTransition");
		Invoke ("BeginCameraTransition", .1f);
	}

	void BeginCameraTransition(){
		// New room bounds
		int left = Mathf.CeilToInt (roomCollider.transform.position.x);
		int top = Mathf.FloorToInt (roomCollider.transform.position.y);
		int width = Utils.PixelsToTiles (Mathf.RoundToInt (roomCollider.size.x)) - 1;
		int height = Utils.PixelsToTiles (Mathf.RoundToInt (roomCollider.size.y)) - 1;
		
		SetRoomRect(left, top, width, height);

		// Pan the cameras
		leftCamera.BeginRoomTransitionPan(CameraTransitionFinished);
		rightCamera.BeginRoomTransitionPan(CameraTransitionFinished);

		// Disable player movement
		leftPlayer.DisableMovement();
		rightPlayer.DisableMovement();
	}

	void EndCameraTransition(){
		leftPlayer.EnableMovement();
		rightPlayer.EnableMovement();
	}

	// Wait for two cameras to finish, then enable player input
	void CameraTransitionFinished(){
		cameraFinishes++;
		if(cameraFinishes == 2){
			cameraFinishes = 0;
			EndCameraTransition();
		}
	}


}
