using UnityEngine;
using System.Collections;

public class RoomManagerScript : MonoBehaviour {

	CameraScript leftCamera, rightCamera;
	PlayerControllerScript leftPlayer, rightPlayer;

	Vector2 _roomCenter;
	public Vector2 roomCenter{
		get{
			return _roomCenter;
		}
	}

	Vector2 _roomSize;
	public Vector2 roomSize{
		get{
			return _roomSize;
		}
	}


	public int roomTop{get;set;}
	public int roomBot{get;set;}
	public int roomLeft{get;set;}
	public int roomRight{get;set;}

	int cameraFinishes;


	void Start () {
		leftCamera = GameObject.FindGameObjectWithTag("LeftCamera").GetComponent<CameraScript>();
		rightCamera = GameObject.FindGameObjectWithTag("RightCamera").GetComponent<CameraScript>();

		leftPlayer = GameObject.FindGameObjectWithTag("PlayerLeft").GetComponent<PlayerControllerScript>();
		rightPlayer = GameObject.FindGameObjectWithTag("PlayerRight").GetComponent<PlayerControllerScript>();

		cameraFinishes = 0;

		SetBounds (-4, 4, 4, -5);
//		SetBounds(-1, 1, 1, -1);

	}

	void Update(){

	}

	public void Reset(){
		SetBounds (-4, 4, 4, -5);

		leftCamera.BeginRoomTransitionPan(CameraTransitionFinished);
		rightCamera.BeginRoomTransitionPan(CameraTransitionFinished);
	}


	void SetCenter(float cx, float cy){
		_roomCenter = new Vector2(cx, cy);
		UpdateRoomBounds();

		LogRoomInfo();
	}

	void SetBounds(int left, int top, int right, int bot){
		roomLeft = left;
		roomTop = top;
		roomRight = right;
		roomBot = bot;

		_roomSize = new Vector2(right - left + 1, top - bot + 1);

		UpdateRoomCenter();

		LogRoomInfo();
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

	void CameraTransitionFinished(){
		cameraFinishes++;
		if(cameraFinishes == 2){
			cameraFinishes = 0;
			EnablePlayerInput();
		}
	}

	public void MoveScreen(Direction direction){

		// Need to wait a short bit before disable input
		// because one player will get stuck if this is called earlier
		CancelInvoke("DisablePlayerInput");
		Invoke ("DisablePlayerInput", .1f);

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
		LogRoomInfo();

		leftCamera.BeginRoomTransitionPan(CameraTransitionFinished);
		rightCamera.BeginRoomTransitionPan(CameraTransitionFinished);
	}
}
