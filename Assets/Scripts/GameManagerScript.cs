using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	CameraScript leftCamera, rightCamera;
	PlayerControllerScript leftPlayer, rightPlayer;
	
	void Start () {
		leftCamera = Globals.cameraLeft;
		rightCamera = Globals.cameraRight;
		leftPlayer = Globals.playerLeft;
		rightPlayer = Globals.playerRight;
	}

	public void RunDisableTest(){
		FadeDownLeftSide();
		Invoke ("FadeUpLeftSide", 8f);
	}

	public void FadeDownRightSide(){
		rightCamera.BeginFadeDown(DisableRightCharacter);
	}
	void DisableRightCharacter(){
		rightPlayer.DisableCharacter();
	}

	public void FadeDownLeftSide(){
		leftCamera.BeginFadeDown(DisableLeftCharacter);
	}

	void DisableLeftCharacter(){
		leftPlayer.DisableCharacter();
	}

	public void FadeUpRightSide(){
		rightCamera.BeginFadeUp(EnableRightCharacter);
	}

	void EnableRightCharacter(){
		rightPlayer.x = leftPlayer.x;
		rightPlayer.y = leftPlayer.y;
		rightPlayer.EnableCharacter();
	}

	public void FadeUpLeftSide(){
		leftCamera.BeginFadeUp(EnableLeftCharacter);
	}

	void EnableLeftCharacter(){
		leftPlayer.x = rightPlayer.x;
		leftPlayer.y = rightPlayer.y;
		leftPlayer.EnableCharacter();
	}



}
