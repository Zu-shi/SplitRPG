using UnityEngine;
using System.Collections;

public class CreditsCutsceneScript : CutsceneScript {

	private void LoadMainMenu() {
		Globals.levelManager.LoadMainMenu();
	}

	protected override IEnumerator ActionSequence() {
		float waitTime = 0;

		SetupScene();

		this.callback = (Utils.VoidDelegate)LoadMainMenu;

		Move(leftPlayer, Direction.UP, 0);
		Move(rightPlayer, Direction.UP, 0);

		leftPlayer.GetComponentInChildren<CharacterWalkingAnimationScript>().enabled = false;
		rightPlayer.GetComponentInChildren<CharacterWalkingAnimationScript>().enabled = false;

		PlayAnimation(leftPlayer, "WalkUpAnimation");
		PlayAnimation(rightPlayer, "WalkUpAnimation");

		yield return new WaitForSeconds(30);

		TearDownScene();

		End();

	}

}
