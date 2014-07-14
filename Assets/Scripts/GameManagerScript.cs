using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

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
	

	void Start () {
		leftCamera = GameObject.FindGameObjectWithTag("LeftCamera").GetComponent<CameraScript>();
		rightCamera = GameObject.FindGameObjectWithTag("RightCamera").GetComponent<CameraScript>();

		leftPlayer = GameObject.FindGameObjectWithTag("PlayerLeft").GetComponent<PlayerControllerScript>();
		rightPlayer = GameObject.FindGameObjectWithTag("PlayerRight").GetComponent<PlayerControllerScript>();

		Reset();
	}

	void Update(){
		leftCamera.center = _roomCenter;
		rightCamera.center = _roomCenter;
	}

	public void Reset(){
		_roomCenter = new Vector2(0, -.5f);
		_roomSize = new Vector2(9, 10);
		
		UpdateRoomBounds();
	}


	void UpdateRoomBounds(){
		roomTop = (int)Mathf.Floor(_roomCenter.y + _roomSize.y / 2);
		roomBot = (int)Mathf.Ceil(_roomCenter.y - _roomSize.y / 2);
		roomRight = (int)Mathf.Floor(_roomCenter.x + _roomSize.x / 2);
		roomLeft = (int)Mathf.Ceil(_roomCenter.x - _roomSize.x / 2);

//		Debug.Log(roomTop + " " + roomBot + " " + roomLeft + " " + roomRight);
	}

	void DisablePlayerInput(){
		// Disallow moving player for a short time
		float moveTime = 1f;
		leftPlayer.DisableMovement(moveTime);
		rightPlayer.DisableMovement(moveTime);
	}
	
	public void MoveScreen(Direction direction){

		CancelInvoke("DisablePlayerInput");
		Invoke ("DisablePlayerInput", .1f);

		// Determine which way to move the center of the screen
		int hMul = 0;
		int vMul = 0;

		switch(direction){
		case Direction.LEFT:
			hMul = -1;
			break;
		case Direction.RIGHT:
			hMul = 1;
			break;
		case Direction.UP:
			vMul = 1;
			break;
		case Direction.DOWN:
			vMul = -1;
			break;

		}

		Vector2 move = new Vector2(_roomSize.x, _roomSize.y);
		move.Scale(new Vector2(hMul, vMul));
		_roomCenter += move;

		UpdateRoomBounds();
	}
}
