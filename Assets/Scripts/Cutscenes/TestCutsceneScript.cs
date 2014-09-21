using UnityEngine;
using System.Collections;

public class TestCutsceneScript : CutsceneScript {
	
	protected override void ActionSequence() {
		float waitTime = 0;
		waitTime = Move(leftPlayer, Direction.UP, 2);
		Debug.Log("Waiting...");
		Wait(waitTime);
		waitTime = Move(rightPlayer, Direction.UP, 2);
		Debug.Log("Waiting...");
		Wait(waitTime);
		Debug.Log("Ending...");
		End();
	}
}
