using UnityEngine;
using System.Collections;

public class ContemplationCutsceneScript : CutsceneScript {
	
	[Tooltip("All wait times are some multiple of this.")]
	public float standardBubbleDisplayTime = 2.0f;
	public float cameraFadeRate = 0.1f;

	public GameObject girlFaceBubble;
	public GameObject boyFaceBubble;
	public GameObject eatingDinerBubble;
	public GameObject playingVideoGamesBubble;
	public GameObject sittingOnParkBenchBubble;
	public GameObject sittingOnTrainBubble;
	public GameObject sadFaceBubbleLeftTail;
	public GameObject sadFaceBubbleRightTail;
	
	protected override IEnumerator ActionSequence() {
		float waitTime = 0;
		GameObject b, a;
		
		CheckPrefabLinks();

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

		// Fade down right camera to 50%
		float tmp = rightCamera.fader.fadeRate;
		rightCamera.fader.fadeRate = cameraFadeRate;
		rightCamera.fader.Dim();
		waitTime = rightCamera.fader.EstimateTime();
		yield return new WaitForSeconds(waitTime);

		// Girl thinks about things they did together
		b = ShowSpeechBubble(leftPlayer, eatingDinerBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);

		b = ShowSpeechBubble(leftPlayer, playingVideoGamesBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Fade down right camera to 50%
		rightCamera.fader.FadeUp();
		waitTime = rightCamera.fader.EstimateTime();
		leftCamera.fader.fadeRate = cameraFadeRate;
		leftCamera.fader.Dim();
		yield return new WaitForSeconds(waitTime);

		// Boy thinks about things they did together
		b = ShowSpeechBubble(rightPlayer, sittingOnParkBenchBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(rightPlayer, sittingOnTrainBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Fade left camera back up
		leftCamera.fader.FadeUp();
		waitTime = leftCamera.fader.EstimateTime();
		yield return new WaitForSeconds(waitTime);

		// Boy and Girl are sad
		b = ShowSpeechBubble(rightPlayer, sadFaceBubbleRightTail);
		a = ShowSpeechBubble(leftPlayer, sadFaceBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime * 2.0f);

		HideSpeechBubble(a); HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Cleanup
		leftCamera.fader.fadeRate = tmp;
		rightCamera.fader.fadeRate = tmp;

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
