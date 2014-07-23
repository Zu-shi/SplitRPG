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

	[Tooltip("Which test to run?")]
	public TestType type;
	
	void Start () {
		Invoke ("RunTest", .5f);
	}

	void RunTest(){

		switch(type){
		case TestType.CAMERA_CUTSCENE:
			Globals.cameraLeft.RunCutsceneTest();
			Globals.cameraRight.RunCutsceneTest();
			break;
		case TestType.DISABLE_SIDE:
			Globals.gameManager.RunDisableTest();
			break;
		case TestType.SMALL_ROOM:
			Globals.roomManager.RunTinyRoomTest();
			break;
		case TestType.BIG_ROOM:
			Globals.roomManager.RunBigRoomTest();
			break;
		}
	}
	

}
