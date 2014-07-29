using UnityEngine;
using System.Collections;

public class RoomManagerScript : MonoBehaviour {

	public bool fadeTransition = false;

	CameraScript leftCamera, rightCamera;
	PlayerControllerScript leftPlayer, rightPlayer;

	BoxCollider2D roomCollider, roomColliderPrev;
	private int roomLayerMasks;

	bool needsTransition;

	// Rect that defines the room (measured in tiles)
	Rect _roomRect;
	
	/// <summary>
	/// The center of the current room
	/// </summary>
	public Vector2 roomCenter{
		get{ return _roomRect.center; }
	}
	/// <summary>
	/// The dimensions of the current room in terms of how many tiles fit inside the room
	/// </summary>
	public Vector2 roomTileDimensions{
		get{ return _roomRect.size; }
	}

	/// <summary>
	/// The top-most tile that is inside the room.
	/// </summary>
	public int roomTop{
		get{ return Mathf.RoundToInt(_roomRect.yMax); }
	}
	/// <summary>
	/// The bottom-most tile that is inside the room.
	/// </summary>
	public int roomBotTile{
		get{ return Mathf.RoundToInt(_roomRect.yMin + 1); }
	}

	/// <summary>
	/// Bot bound of the room (one unit down from the bottom tile)
	/// </summary>
	public int roomBot{
		get{ return Mathf.RoundToInt(_roomRect.yMin); }
	}

	/// <summary>
	/// The left-most tile that is inside the room.
	/// </summary>
	public int roomLeft{
		get{ return Mathf.RoundToInt(_roomRect.xMin); }
	}
	/// <summary>
	/// The right-most tile that is inside the room.
	/// </summary>
	public int roomRightTile{
		get{ return Mathf.RoundToInt(_roomRect.xMax - 1); }
	}

	/// <summary>
	/// Right bound of the room (one unit right of the far right tile)
	/// </summary>
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

		needsTransition = false;
		cameraFinishes = 0;

		// Standard 9 x 10 
		SetRoomRect(0,0,0,0);
	}

	void Update() {
		if(needsTransition){
			BeginCameraTransition();
			needsTransition = false;
		}

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
	}
	
	/// <summary>
	/// Whether the tile is inside the room
	/// </summary>
	public bool RoomContainsTile(Vector2 tile){
		if(tile.x >= roomLeft && tile.x <= roomRightTile && tile.y >= roomBotTile && tile.y <= roomTop){
			return true;
		} else {
			return false;
		}
	}
	
	/// <summary>
	/// Sets the room rect.
	/// </summary>
	/// <param name="left">Left-most tile.</param>
	/// <param name="top">Top-most tile.</param>
	/// <param name="width">Width e.g. 9 if there are 8 spaces the player can be</param>
	/// <param name="height">Height e.g. 9 if there are 8 spaces the player can be</param>
	void SetRoomRect(float left, float top, float width, float height){
		// Rect objects use a GUI coordinate system where the y axis is opposite than in 3D view
		// So we actually pass in (left, BOT, width, height) instead of (left, TOP...)
		_roomRect = new Rect(left, top-height, width, height);
	}

	void LogRoomInfo(){
		Debug.Log("Room Center = (" + roomCenter.x + ", " + roomCenter.y + 
		          ")   Tile Dimensions = " + roomTileDimensions.x + " x " + roomTileDimensions.y);
		Debug.Log ("Room Bounds: (" + roomLeft + ", " + roomTop + ") to (" + roomRight + ", " + roomBot + ")");
	}

	/// <summary>
	/// Call when the screen needs to be moved to another room
	/// </summary>
	void MoveScreen(){
		// Need to wait a short bit before transitioning 
		// because one character will get stuck if he hasn't moved yet
		needsTransition = true;
	}

	/// <summary>
	/// Resets the bounds of the room, moves the cameras, disables movement for players
	/// </summary>
	void BeginCameraTransition(){
		// New room bounds
		int left = Mathf.RoundToInt (roomCollider.transform.position.x);
		int top = Mathf.RoundToInt (roomCollider.transform.position.y);
		int width = Utils.PixelsToTiles (Mathf.RoundToInt (roomCollider.size.x));
		int height = Utils.PixelsToTiles (Mathf.RoundToInt (roomCollider.size.y));
		
		SetRoomRect(left, top, width, height);

		// Pan the cameras
		if(!fadeTransition) {
			leftCamera.BeginRoomTransitionPan(CameraTransitionFinished);
			rightCamera.BeginRoomTransitionPan(CameraTransitionFinished);
		}
		else {
			leftCamera.BeginRoomTransitionFade(CameraTransitionFinished);
			rightCamera.BeginRoomTransitionFade(CameraTransitionFinished);
		}

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
