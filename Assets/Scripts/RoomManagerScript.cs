using UnityEngine;
using System.Collections;

public class RoomManagerScript : MonoBehaviour {

	CameraScript leftCamera, rightCamera;
	PlayerControllerScript leftPlayer, rightPlayer;

	// Center of room in tiles
	Vector2 _roomCenter;
	public Vector2 roomCenter{
		get{
			return _roomCenter;
		}
	}

	// Size of room in tiles
	Vector2 _roomSize;
	public Vector2 roomSize{
		get{
			return _roomSize;
		}
	}

	// Outer tile coords of a room
	public int roomTop{get;set;}
	public int roomBot{get;set;}
	public int roomLeft{get;set;}
	public int roomRight{get;set;}

	// Track how many cameras have finished their transition
	int cameraFinishes;


	void Start () {
		leftCamera = GameObject.FindGameObjectWithTag("LeftCamera").GetComponent<CameraScript>();
		rightCamera = GameObject.FindGameObjectWithTag("RightCamera").GetComponent<CameraScript>();

		leftPlayer = GameObject.FindGameObjectWithTag("PlayerLeft").GetComponent<PlayerControllerScript>();
		rightPlayer = GameObject.FindGameObjectWithTag("PlayerRight").GetComponent<PlayerControllerScript>();

		cameraFinishes = 0;

		SetBounds (-4, 4, 4, -5); // Standard
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
		SetBounds(-9, 10, 9, -10);
	}

	void SetCenter(float cx, float cy, float sx, float sy){
		_roomSize = new Vector2(sx, sy);
		_roomCenter = new Vector2(cx, cy);
		UpdateRoomBounds();
	}

	void SetBounds(int left, int top, int right, int bot){
		roomLeft = left;
		roomTop = top;
		roomRight = right;
		roomBot = bot;

		_roomSize = new Vector2(right - left + 1, top - bot + 1);

		UpdateRoomCenter();
	}

	void UpdateRoomBounds(){
		roomTop = (int)Mathf.Floor(_roomCenter.y + _roomSize.y / 2);
		roomBot = (int)Mathf.Ceil(_roomCenter.y - _roomSize.y / 2);
		roomRight = (int)Mathf.Floor(_roomCenter.x + _roomSize.x / 2);
		roomLeft = (int)Mathf.Ceil(_roomCenter.x - _roomSize.x / 2);
	}

	void UpdateRoomCenter(){
		float cx = (roomLeft + roomRight)/2.0f;
		float cy = (roomTop + roomBot)/2.0f;
		_roomCenter = new Vector2(cx, cy);
	}

	void LogRoomInfo(){
		Debug.Log("Room Center = (" + _roomCenter.x + ", " + _roomCenter.y + 
		          ")   Size = " + _roomSize.x + " x " + _roomSize.y);
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
	// In the future we'll read the room data from a file but right now
	// it just moves over by one room width
	public void MoveScreen(Direction direction){

		// Need to wait a short bit before disable input
		// because one character will get stuck if he hasn't moved yet
		CancelInvoke("DisablePlayerInput");
		Invoke ("DisablePlayerInput", .1f);

		// Move the room center in the correct direction
		switch(direction){
		case Direction.LEFT:
			_roomCenter += new Vector2(-_roomSize.x, 0);
			break;
		case Direction.RIGHT:
			_roomCenter += new Vector2(_roomSize.x, 0);
			break;
		case Direction.UP:
			_roomCenter += new Vector2(0, _roomSize.y);
			break;
		case Direction.DOWN:
			_roomCenter += new Vector2(0, -_roomSize.y);
			break;
		}

		UpdateRoomBounds();
//		LogRoomInfo();

		// Pan the cameras
		leftCamera.BeginRoomTransitionPan(CameraTransitionFinished);
		rightCamera.BeginRoomTransitionPan(CameraTransitionFinished);
	}
}
