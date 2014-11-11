using UnityEngine;
using System.Collections;

public class CreditsCutsceneScript : CutsceneScript {

	public AudioClip creditsTheme;
	//private bool scopingCamera = false;

	private void LoadMainMenu() {
		Globals.levelManager.LoadMainMenu();
	}

	protected override IEnumerator ActionSequence() {
		skipable = false;
		float waitTime = 0;
		
		Move(leftPlayer, Direction.RIGHT, 0);
		Move(rightPlayer, Direction.LEFT, 0);

		Globals.gameManager.SetScopeCamera(true);
		Globals.line.FadeDown();

		yield return new WaitForSeconds(4.0f);
		
		Globals.gameManager.SetScopeCamera(false);

		Move(leftPlayer, Direction.UP, 0);
		Move(rightPlayer, Direction.UP, 0);

		yield return new WaitForSeconds(2.0f);

		Move(leftPlayer, Direction.UP, 3);
		Move(rightPlayer, Direction.UP, 3);

		Color a = Color.white;
		a.a = 0.0f;
		rightCamera.fader.GetComponent<GUITexture>().color = a;
		leftCamera.fader.GetComponent<GUITexture>().color = a;

		FadeCameraOut(rightCamera);
		waitTime = FadeCameraOut(leftCamera);

		yield return new WaitForSeconds(waitTime);

		SetupScene();

		FadeCameraIn(rightCamera);
		waitTime = FadeCameraIn(leftCamera);
		
		yield return new WaitForSeconds(waitTime);

		a = Color.black;
		a.a = 0;
		rightCamera.fader.GetComponent<GUITexture>().color = a;
		leftCamera.fader.GetComponent<GUITexture>().color = a;

		this.callback = (Utils.VoidDelegate)LoadMainMenu;

		Move(leftPlayer, Direction.UP, 0);
		Move(rightPlayer, Direction.UP, 0);

		leftPlayer.GetComponentInChildren<CharacterWalkingAnimationScript>().enabled = false;
		rightPlayer.GetComponentInChildren<CharacterWalkingAnimationScript>().enabled = false;

		PlayAnimation(leftPlayer, "WalkUpAnimation");
		PlayAnimation(rightPlayer, "WalkUpAnimation", 2);
		//rightPlayer.transform.GetChild(0).gameObject.GetComponent<SpriteAnimationManagerScript>();

		Globals.soundManager.LoadAndPlayClip(creditsTheme);
		Globals.soundManager.audio.loop = false;

		yield return new WaitForSeconds(360000);

		TearDownScene();

		End();

	}

}
