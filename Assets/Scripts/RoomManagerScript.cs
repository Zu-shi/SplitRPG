using UnityEngine;
using System.Collections;

public class RoomManagerScript : MonoBehaviour {
	
	CameraScript leftCamera, rightCamera;
	PlayerControllerScript leftPlayer, rightPlayer;

	BoxCollider2D cameraCollider, cameraColliderPrev;
	private int cameraLayerMasks;

	// Rect that defines the room (measured in tiles)
	Rect _roomRect;
	
	// "Helper" Properties
	public Vector2 roomCenter{
		get{ return _roomRect.center; }
	}
	public Vector2 roomSize{
		get{ return _roomRect.size; }
	}
	public int roomTop{
		get{ return Utils.Round(_roomRect.yMax); }
	}
	public int roomBot{
		get{ return Utils.Round(_roomRect.yMin); }
	}
	public int roomLeft{
		get{ return Utils.Round(_roomRect.xMin); }
	}
	public int roomRight{
		get{ return Utils.Round(_roomRect.xMax); }
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
		//Check for only the left and right camera layers
		cameraLayerMasks = ( (1 << LayerMask.NameToLayer("RightCamera")) | 
		                    (1 << LayerMask.NameToLayer("LeftCamera")) );

		cameraCollider = (BoxCollider2D) Physics2D.Raycast(new Vector2(leftPlayer.x, leftPlayer.y), 
		                                             Vector2.zero, 
		                                             0f, 
		                                             cameraLayerMasks).collider;

		if (cameraCollider != cameraColliderPrev) {
			MoveScreen();
		}

		cameraColliderPrev = cameraCollider;
	}

	// Temprary hack to reset the camera when the player falls off the level
	public void Reset(){
		leftCamera.BeginRoomTransitionFade(CameraTransitionFinished);
		rightCamera.BeginRoomTransitionFade(CameraTransitionFinished);

		SetBounds (-4, -5, 4, 4);
	}

	public void RunTinyRoomTest(){
		SetBounds(-2, -2, 2, 2); 
	}

	public void RunBigRoomTest(){
		SetBounds(-9, -10, 10, 10);
	}

	void SetRoomRect(float left, float top, float width, float height){
		// Rect objects use a GUI coordinate system where the y axis is opposite than in 3D view
		// So we actually pass in (left, BOT, width, height) instead of (left, TOP...)
		_roomRect = new Rect(left, top-height, width, height);
	}

	void SetBounds(int left, int bot, int right, int top){
		SetRoomRect(left, top, right-left, top-bot);
	}

	void LogRoomInfo(){
		Debug.Log("Room Center = (" + roomCenter.x + ", " + roomCenter.y + 
		          ")   Size = " + roomSize.x + " x " + roomSize.y);
		Debug.Log ("Room Bounds: (" + roomLeft + ", " + roomTop + ") to (" + roomRight + ", " + roomBot + ")");
	}

	void DisablePlayerInput(){
		leftPlayer.DisableMovement();
		rightPlayer.DisableMovement();
	}

	void EnablePlayerInput(){
		leftPlayer.EnableMovement();
		rightPlayer.EnableMovement();
	}

	// Wait for two cameras to finish, then enable player input
	void CameraTransitionFinished(){
		cameraFinishes++;
		if(cameraFinishes == 2){
			cameraFinishes = 0;
			EnablePlayerInput();
		}
	}

	// Called when both characters walk off the side of the screen
	public void MoveScreen(){

		// Need to wait a short bit before disable input
		// because one character will get stuck if he hasn't moved yet
		CancelInvoke("DisablePlayerInput");
		Invoke ("DisablePlayerInput", .1f);

		// New room bounds
		SetBounds (Mathf.CeilToInt (cameraCollider.transform.position.x), 
		           Mathf.CeilToInt (cameraCollider.transform.position.y) - Utils.PixelsToTiles (Mathf.RoundToInt (cameraCollider.size.y)),
		           Mathf.CeilToInt (cameraCollider.transform.position.x) + Utils.PixelsToTiles (Mathf.RoundToInt (cameraCollider.size.x)) - 1,
		           Mathf.CeilToInt (cameraCollider.transform.position.y) - 1);

		// Pan the cameras
		leftCamera.BeginRoomTransitionPan(CameraTransitionFinished);
		rightCamera.BeginRoomTransitionPan(CameraTransitionFinished);
	}
}
