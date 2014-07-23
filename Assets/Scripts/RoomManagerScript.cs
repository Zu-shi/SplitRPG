using UnityEngine;
using System.Collections;

public class RoomManagerScript : MonoBehaviour {

	public BoxCollider2D collider;
	CameraScript leftCamera, rightCamera;
	PlayerControllerScript leftPlayer, rightPlayer;
	private int CameraLayerMasks;

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
		leftCamera = GameObject.FindGameObjectWithTag("LeftCamera").GetComponent<CameraScript>();
		rightCamera = GameObject.FindGameObjectWithTag("RightCamera").GetComponent<CameraScript>();

		leftPlayer = GameObject.FindGameObjectWithTag("PlayerLeft").GetComponent<PlayerControllerScript>();
		rightPlayer = GameObject.FindGameObjectWithTag("PlayerRight").GetComponent<PlayerControllerScript>();

		cameraFinishes = 0;

		// Standard 8 x 9 
		SetBounds (-4, 4, 4, -5); 
	}

	void Update() {
		//CameraLayerMasks = ( (1 << LayerMask.NameToLayer("RightCamera")) | 
		//                    (1 << LayerMask.NameToLayer("LeftCamera")) );
		//collider = (BoxCollider2D)Physics2D.Raycast(new Vector2(leftPlayer.x, leftPlayer.y), Vector2.zero, 0f, CameraLayerMasks).collider;
		//collider = (BoxCollider2D)Physics2D.Raycast(new Vector2(leftPlayer.x, leftPlayer.y), Vector2.zero, 0f).collider;
		//Debug.Log (Physics2D.Raycast(new Vector2(leftPlayer.x, leftPlayer.y), Vector2.zero, 0f).collider, CameraLayerMasks);
		//Debug.Log ("Collider center:" + collider.center);
		//Debug.Log ("Collider position:" + collider.transform.position);
		//Debug.Log ("Collider size:" + collider.size);
	}

	// Temprary hack to reset the camera when the player falls off the level
	public void Reset(){
		leftCamera.BeginRoomTransitionFade(CameraTransitionFinished);
		rightCamera.BeginRoomTransitionFade(CameraTransitionFinished);

		SetBounds (-4, 4, 4, -5);
	}

	public void RunTinyRoomTest(){
		SetBounds(-2, 2, 2, -2); 
	}

	public void RunBigRoomTest(){
		SetBounds(-9, 10, 10, -10);
	}

	void SetRoomRect(float left, float top, float width, float height){
		// Rect objects use a GUI coordinate system where the y axis is opposite than in 3D view
		// So we actually pass in (left, BOT, width, height) instead of (left, TOP...)
		_roomRect = new Rect(left, top-height, width, height);
	}

	void SetBounds(int left, int top, int right, int bot){
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
	public void MoveScreen(Direction direction){

		// Need to wait a short bit before disable input
		// because one character will get stuck if he hasn't moved yet
		CancelInvoke("DisablePlayerInput");
		Invoke ("DisablePlayerInput", .1f);


		// Set room bounds to new room

		// *** REPLACE THIS CODE *** // 

		// We should read the room data from a file but right now
		// we just move over by one room width
		float dx = 0, dy = 0;
		switch(direction){
		case Direction.LEFT:
			dx = -roomSize.x - 1;
			break;
		case Direction.RIGHT:
			dx = roomSize.x + 1;
			break;
		case Direction.UP:
			dy = roomSize.y + 1;
			break;
		case Direction.DOWN:
			dy = -roomSize.y - 1;
			break;
		}

		SetRoomRect(roomLeft + dx, roomTop + dy, roomSize.x, roomSize.y);

		// *** STOP REPLACING *** /// 


//		LogRoomInfo();

		// Pan the cameras
		leftCamera.BeginRoomTransitionPan(CameraTransitionFinished);
		rightCamera.BeginRoomTransitionPan(CameraTransitionFinished);
	}
}
