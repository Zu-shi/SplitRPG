using UnityEngine;
using System.Collections;

public class ContemplationCutsceneScript : CutsceneScript {
	
	[Tooltip("All wait times are some multiple of this.")]
	public float standardBubbleDisplayTime = 2.0f;

	public GameObject girlFaceBubble;
	public GameObject boyFaceBubble;
	public GameObject eatingDinerBubble;
	public GameObject playingVideoGamesBubble;
	public GameObject sittingOnParkBenchBubble;
	public GameObject sittingOnTrainBubble;
	public GameObject sadFaceBubbleLeftTail;
	public GameObject sadFaceBubbleRightTail;

	private void MakeCharactersVisible(bool visible) {
		leftPlayer.GetComponentInChildren<SpriteRenderer>().enabled = visible;
		rightPlayer.GetComponentInChildren<SpriteRenderer>().enabled = visible;
	}

	protected override IEnumerator ActionSequence() {
		float waitTime = 0;
		GameObject b, a;
		
		CheckPrefabLinks();
		SetupScene();
		MakeCharactersVisible(false);

		float tmp = rightCamera.fader.fadeRate;

		rightCamera.fader.fadeRate = fadeRate;
		leftCamera.fader.fadeRate = fadeRate;

		yield return new WaitForSeconds(standardBubbleDisplayTime);

		// Girl thinks about boy
		b = ShowSpeechBubble(leftPlayer, boyFaceBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		// Boy thinks about girl
		b = ShowSpeechBubble(rightPlayer, girlFaceBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		// Girl thinks about things they did together
		b = ShowSpeechBubble(leftPlayer, eatingDinerBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);

		b = ShowSpeechBubble(leftPlayer, playingVideoGamesBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Boy thinks about things they did together
		b = ShowSpeechBubble(rightPlayer, sittingOnParkBenchBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(rightPlayer, sittingOnTrainBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Boy and Girl are sad
		b = ShowSpeechBubble(rightPlayer, sadFaceBubbleRightTail);
		a = ShowSpeechBubble(leftPlayer, sadFaceBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime * 2.0f);

		HideSpeechBubble(a); HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		waitTime = FadeCameraOut(rightCamera);
		FadeCameraOut(leftCamera);
		yield return new WaitForSeconds(waitTime);

		TearDownScene();
		MakeCharactersVisible(true);

		waitTime = FadeCameraIn(rightCamera);
		FadeCameraIn(leftCamera);
		yield return new WaitForSeconds(waitTime);

		rightCamera.fader.fadeRate = tmp;
		leftCamera.fader.fadeRate = tmp;

		End();
	}
	
	private bool CheckPrefabLinks() {
		if(girlFaceBubble == null
		   || boyFaceBubble == null
		   || sittingOnParkBenchBubble == null
		   || eatingDinerBubble == null
		   || playingVideoGamesBubble == null
		   || sadFaceBubbleLeftTail == null
		   || sittingOnTrainBubble == null
		   || sadFaceBubbleRightTail == null
		   ) {
			Debug.LogError("Missing speech bubble assignment.");
			return false;
		}
		return true;
	}
}
