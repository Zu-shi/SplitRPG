using UnityEngine;
using System.Collections;

public class TestRunnerScript : MonoBehaviour {

	public enum TestType {
		NONE,
		CAMERA_CUTSCENE,
		DISABLE_SIDE,
		SMALL_ROOM,
		BIG_ROOM
	}

	public TestType type;

	CameraScript leftCamera, rightCamera;
	GameManagerScript gameManager;
	RoomManagerScript roomManager;


	void Start () {
		leftCamera = GameObject.FindGameObjectWithTag("LeftCamera").GetComponent<CameraScript>();
		rightCamera = GameObject.FindGameObjectWithTag("RightCamera").GetComponent<CameraScript>();

		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
		roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManagerScript>();

		Invoke ("RunTest", .5f);
	}

	void RunTest(){

		switch(type){
		case TestType.CAMERA_CUTSCENE:
			leftCamera.RunCutsceneTest();
			rightCamera.RunCutsceneTest();
			break;
		case TestType.DISABLE_SIDE:
			gameManager.RunDisableTest();
			break;
		case TestType.SMALL_ROOM:
			roomManager.RunTinyRoomTest();
			break;
		case TestType.BIG_ROOM:
			roomManager.RunBigRoomTest();
			break;
		}
	}
	

}
