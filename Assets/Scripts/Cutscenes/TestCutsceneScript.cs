using UnityEngine;
using System.Collections;

/// <summary>
/// Test cutscene script.
/// </summary>
/// <author>Tyler Wallace</author>
public class TestCutsceneScript : CutsceneScript {

	public GameObject testBubble1;
	public GameObject testBubble2;

	protected override IEnumerator ActionSequence() {
		// Init
		float waitTime = 0;
		if(testBubble1 == null) {
			Debug.LogError("No speech bubble assigned!");
		}
		if(testBubble2 == null) {
			Debug.LogError("No speech bubble assigned!");
		}

		// Both characters walk up two tiles
		waitTime = Move(leftPlayer, Direction.UP, 2);
		waitTime = Move(rightPlayer, Direction.UP, 2);	// Clobbers other wait time. Use max for better results.
		yield return new WaitForSeconds(waitTime);		// We wait until the characters finish moving.

		// Both characters walk right 4 tiles
		waitTime = Move(leftPlayer, Direction.RIGHT, 4);
		waitTime = Move(rightPlayer, Direction.RIGHT, 4);
		yield return new WaitForSeconds(waitTime);

		// Both characters face up
		Move(leftPlayer, Direction.UP, 0);
		Move(rightPlayer, Direction.UP, 0);

		// Fade the cameras out
		waitTime = FadeCameraOut(leftCamera);
		waitTime = FadeCameraOut(rightCamera);
		yield return new WaitForSeconds(waitTime);

		// Both characters move right 2 tiles
		waitTime = Move(leftPlayer, Direction.LEFT, 2);
		waitTime = Move(rightPlayer, Direction.LEFT, 2);
		yield return new WaitForSeconds(waitTime);

		// Both characters face down
		Move(leftPlayer, Direction.DOWN, 0);
		Move(rightPlayer, Direction.DOWN, 0);
		yield return new WaitForSeconds(0.5f);

		// Fade the cameras in
		waitTime = FadeCameraIn(leftCamera);
		waitTime = FadeCameraIn(rightCamera);
		yield return new WaitForSeconds(waitTime);
		
		// Left character "talks" for a short time
		GameObject bubble1 = ShowSpeechBubble(leftPlayer, testBubble1);
		yield return new WaitForSeconds(1.7f);
		
		// Left character stops talking
		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(0.2f);

		// Right character "talks" for a short time
		GameObject bubble2 = ShowSpeechBubble(rightPlayer, testBubble2);
		yield return new WaitForSeconds(1.7f);
		
		// Left character stops talking
		HideSpeechBubble(bubble2);
		yield return new WaitForSeconds(0.2f);

		// End the cutscene. This must be called at the end of every cutscene.
		End();
	}
}
