using UnityEngine;
using System.Collections;

public class TestRunnerScript : MonoBehaviour {

	public enum TestType {
		NONE,
		CAMERA_CUTSCENE,
		DISABLE_SIDE,
		ACTION_TEST
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
			break;;
		case TestType.ACTION_TEST:
			TimerAction.Create(2, TimerCallback).StartAction();
			break;
		}
	}

	void TimerCallback(){
		Debug.Log ("Action test successful.");
	}
	

}
