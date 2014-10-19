using UnityEngine;
using System.Collections;

public class TrainCutsceneScript : CutsceneScript {

	public string sittingAnimationName;

	[Tooltip("All wait times are some multiple of this.")]
	public float standardBubbleDisplayTime = 2.0f;

	public float fadeRate = 0.5f;

	public GameObject askForSeatBubble;
	public GameObject smileyFaceBubbleLeftTail;
	public GameObject smileyFaceBubbleRightTail;
	public GameObject isTheSceneryGoodBubble;
	public GameObject theSceneryIsGoodBubble;
	public GameObject isTheMountainGoodBubble;
	public GameObject theMountainIsGoodBubble;
	public GameObject doYouLikeVideoGamesBubble;
	public GameObject iLikeVideoGamesBubble;
	public GameObject wizardBubble;
	public GameObject dragonBubbleLeftTail;
	public GameObject dragonBubbleRightTail;
	public GameObject iLikeSciFiBubbleRightTail;
	public GameObject iLikeSciFiBubbleLeftTail;
	public GameObject spaceSuitBubble;
	public GameObject ellipsiesBubbleLeftTail;
	public GameObject ellipsiesBubbleRightTail;
	public GameObject coffeeShopBubble;
	public GameObject fivePmBubble;

	protected override IEnumerator ActionSequence() {
		float waitTime = 0;

		if(sittingAnimationName == null) {
			Debug.LogError("No sitting animation assigned.");
		}

		CheckPrefabLinks();

		SetupScene();

		Move(leftPlayer, Direction.RIGHT, 0);
		PlayAnimation(leftPlayer, "SitRight");
		Move(rightPlayer, Direction.UP, 0);

		yield return new WaitForSeconds(standardBubbleDisplayTime * 2.0f);

		// Boy walks to occupied room
		waitTime = Move(rightPlayer, Direction.LEFT, 5);
		yield return new WaitForSeconds(waitTime);

		waitTime = Move(rightPlayer, Direction.UP, 0);
		yield return new WaitForSeconds(waitTime + standardBubbleDisplayTime);

		// Can I sit here?
		GameObject bubble1 = ShowSpeechBubble(rightPlayer, askForSeatBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Yes, you can sit there.
		bubble1 = ShowSpeechBubble(leftPlayer, smileyFaceBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);

		// Boy moves to seat and sits.
		waitTime = Move(rightPlayer, Direction.UP, 2);
		yield return new WaitForSeconds(waitTime);

		waitTime = Move(rightPlayer, Direction.RIGHT, 1);
		yield return new WaitForSeconds(waitTime + standardBubbleDisplayTime / 4.0f);

		// Boy turn around and sit down
		waitTime = Move(rightPlayer, Direction.LEFT, 0);
		yield return new WaitForSeconds(waitTime + standardBubbleDisplayTime / 4.0f);

		PlayAnimation(rightPlayer, "SitLeft");

		// Pause.
		yield return new WaitForSeconds(2.0f * standardBubbleDisplayTime);

		// Do you like the scenery?
		bubble1 = ShowSpeechBubble(leftPlayer, isTheSceneryGoodBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Yes, I like the scenery.
		bubble1 = ShowSpeechBubble(rightPlayer, theSceneryIsGoodBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);

		bubble1 = ShowSpeechBubble(rightPlayer, smileyFaceBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		// Do you like the mountain?
		bubble1 = ShowSpeechBubble(rightPlayer, isTheMountainGoodBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Yes, I like the mountain.
		bubble1 = ShowSpeechBubble(leftPlayer, theMountainIsGoodBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		// Pause
		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime * 2.0f);

		// Do you like video games?
		bubble1 = ShowSpeechBubble(rightPlayer, doYouLikeVideoGamesBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Yes, I like video games, mostly fantasy games.
		bubble1 = ShowSpeechBubble(leftPlayer, iLikeVideoGamesBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);

		bubble1 = ShowSpeechBubble(leftPlayer, wizardBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		bubble1 = ShowSpeechBubble(leftPlayer, dragonBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// I like fantasy too, but I like scifi more!
		bubble1 = ShowSpeechBubble(rightPlayer, dragonBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		bubble1 = ShowSpeechBubble(rightPlayer, iLikeSciFiBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// I like scifi too!
		bubble1 = ShowSpeechBubble(leftPlayer, iLikeSciFiBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		bubble1 = ShowSpeechBubble(rightPlayer, spaceSuitBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		float tmp = rightCamera.fader.fadeRate;
		rightCamera.fader.fadeRate = fadeRate;
		leftCamera.fader.fadeRate = fadeRate;
		waitTime = FadeCameraOut(rightCamera);
		waitTime = FadeCameraOut(leftCamera);
		yield return new WaitForSeconds(waitTime);

		HideSpeechBubble(bubble1);

		waitTime = FadeCameraIn(rightCamera);
		waitTime = FadeCameraIn(leftCamera);
		yield return new WaitForSeconds(waitTime);

		// Somehow signal the train is stopping.
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		PlayAnimation(leftPlayer, "WalkDownAnimation");
		PlayAnimation(rightPlayer, "WalkDownAnimation");

		waitTime = Move(leftPlayer, Direction.RIGHT);
		waitTime = Move(rightPlayer, Direction.LEFT);
		yield return new WaitForSeconds(waitTime);

		waitTime = Move(leftPlayer, Direction.DOWN, 2);
		waitTime = Move(rightPlayer, Direction.DOWN, 2);
		yield return new WaitForSeconds(waitTime);

		waitTime = Move(leftPlayer, Direction.LEFT);
		waitTime = Move(rightPlayer, Direction.RIGHT);
		yield return new WaitForSeconds(waitTime);

		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		Move(leftPlayer, Direction.RIGHT, 0);
		Move(rightPlayer, Direction.LEFT, 0);

		bubble1 = ShowSpeechBubble(leftPlayer, ellipsiesBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		bubble1 = ShowSpeechBubble(rightPlayer, ellipsiesBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		bubble1 = ShowSpeechBubble(leftPlayer, coffeeShopBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		bubble1 = ShowSpeechBubble(rightPlayer, smileyFaceBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		HideSpeechBubble(bubble1);
		bubble1 = ShowSpeechBubble(rightPlayer, fivePmBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		bubble1 = ShowSpeechBubble(leftPlayer, smileyFaceBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		bubble1 = ShowSpeechBubble(rightPlayer, smileyFaceBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(bubble1);
		Move(leftPlayer, Direction.LEFT, 4);
		Move(rightPlayer, Direction.RIGHT, 4);
		yield return new WaitForSeconds(0.5f);

		waitTime = FadeCameraOut(leftCamera);
		FadeCameraOut(rightCamera);
		yield return new WaitForSeconds(waitTime);

		Move(leftPlayer, Direction.UP, 0);
		Move(rightPlayer, Direction.UP, 0);
		TearDownScene();

		waitTime = FadeCameraIn(leftCamera);
		FadeCameraIn(rightCamera);
		yield return new WaitForSeconds(waitTime);

		rightCamera.fader.fadeRate = tmp;
		leftCamera.fader.fadeRate = tmp;

		End();
	}

	private bool CheckPrefabLinks() {
		if(askForSeatBubble == null
		   || smileyFaceBubbleLeftTail == null
		   || smileyFaceBubbleRightTail == null
		   || isTheSceneryGoodBubble == null
		   || theSceneryIsGoodBubble == null
		   || isTheMountainGoodBubble == null
		   || theMountainIsGoodBubble == null
		   || doYouLikeVideoGamesBubble == null
		   || iLikeVideoGamesBubble == null
		   || wizardBubble == null
		   || dragonBubbleLeftTail == null
		   || dragonBubbleRightTail == null
		   || iLikeSciFiBubbleRightTail == null
		   || iLikeSciFiBubbleLeftTail == null
		   || spaceSuitBubble == null
		   || ellipsiesBubbleLeftTail == null
		   || ellipsiesBubbleRightTail == null
		   || coffeeShopBubble == null
		   || fivePmBubble == null
		   ) {
			Debug.LogError("Missing speech bubble assignment.");
			return false;
		}
		return true;
	}
}
