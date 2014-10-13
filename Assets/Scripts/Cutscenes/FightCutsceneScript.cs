using UnityEngine;
using System.Collections;

public class FightCutsceneScript : CutsceneScript {
	
	[Tooltip("All wait times are some multiple of this.")]
	public float standardBubbleDisplayTime = 2.0f;

	public GameObject confusionBubble;
	public GameObject vrHeadSetBubble;
	public GameObject expensiveBubbleLeftTail;
	public GameObject expensiveBubbleRightTail;
	public GameObject smileyFaceBubbleRightTail;
	public GameObject cyberpunkGameBubble;
	public GameObject wizardGameBubble;
	public GameObject playingWithVrBubble;
	public GameObject angryBubbleLeftTail;
	public GameObject angryBubbleRightTail;
	public GameObject breadBubble;
	public GameObject catFoodBubble;
	public GameObject newCarBubble;
	public GameObject exclaimationMarkBubble;
	public GameObject noNewCarBubble;
	public GameObject oldCarBubble;
	public GameObject noOldCarBubble;
	public GameObject elipsesBubbleLeftTail;
	public GameObject elipsesBubbleRightTail;
	
	protected override IEnumerator ActionSequence() {
		GameObject b;
		
		CheckPrefabLinks();

		// Girl walks in on boy playing with VR
		// Stuff?
		// Girl is confused/suprised
		b = ShowSpeechBubble(leftPlayer, confusionBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime);

		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Boy takes off VR headset
		// Stuff?
		// Boy says "Look, it's so cool!"
		b = ShowSpeechBubble(rightPlayer, vrHeadSetBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);

		b = ShowSpeechBubble(rightPlayer, expensiveBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);

		b = ShowSpeechBubble(rightPlayer, smileyFaceBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Girl says "Expensive???"
		b = ShowSpeechBubble(leftPlayer, expensiveBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);

		b = ShowSpeechBubble(leftPlayer, elipsesBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Boy talks about the games he bought.
		b = ShowSpeechBubble(rightPlayer, expensiveBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(rightPlayer, cyberpunkGameBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(rightPlayer, wizardGameBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);

		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(rightPlayer, playingWithVrBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);

		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Girl is angry and talks about other things to buy.
		b = ShowSpeechBubble(leftPlayer, angryBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);

		b = ShowSpeechBubble(leftPlayer, expensiveBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);

		b = ShowSpeechBubble(leftPlayer, breadBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);

		b = ShowSpeechBubble(leftPlayer, catFoodBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(leftPlayer, newCarBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Boy doesn't want a new car.
		b = ShowSpeechBubble(rightPlayer, exclaimationMarkBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(rightPlayer, noNewCarBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(rightPlayer, oldCarBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Girl doesn't like the old car.
		b = ShowSpeechBubble(leftPlayer, noOldCarBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(leftPlayer, newCarBubble);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(leftPlayer, angryBubbleLeftTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Girl storms out.
		// Stuff?
		// Guy is angry.
		b = ShowSpeechBubble(rightPlayer, elipsesBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 8.0f);
		
		b = ShowSpeechBubble(rightPlayer, angryBubbleRightTail);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 1.5f);
		
		HideSpeechBubble(b);
		yield return new WaitForSeconds(standardBubbleDisplayTime / 2.0f);

		// Guy walks out.
		// Stuff?
		End();
	}
	
	private bool CheckPrefabLinks() {
		if(confusionBubble == null
		   || vrHeadSetBubble == null
		   || smileyFaceBubbleRightTail == null
		   || expensiveBubbleLeftTail == null
		   || expensiveBubbleRightTail == null
		   || wizardGameBubble == null
		   || cyberpunkGameBubble == null
		   || playingWithVrBubble == null
		   || angryBubbleLeftTail == null
		   || angryBubbleRightTail == null
		   || breadBubble == null
		   || catFoodBubble == null
		   || newCarBubble == null
		   || exclaimationMarkBubble == null
		   || noNewCarBubble == null
		   || oldCarBubble == null
		   || noOldCarBubble == null
		   || elipsesBubbleLeftTail == null
		   || elipsesBubbleRightTail == null
		   ) {
			Debug.LogError("Missing speech bubble assignment.");
			return false;
		}
		return true;
	}
}
