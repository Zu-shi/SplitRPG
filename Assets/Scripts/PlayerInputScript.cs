using UnityEngine;
using System.Collections;

public class PlayerInputScript : MonoBehaviour {


	PlayerControllerScript pcLeft, pcRight;

	InputManagerScript inputManager;

	Direction _inputDirection;

	public Direction inputDirection {
		get {
			return _inputDirection;
		}
	}


	void Start () {
		_inputDirection = Direction.NONE;

		// Find Players
		GameObject leftPlayer = GameObject.FindGameObjectWithTag("PlayerLeft");
		GameObject rightPlayer = GameObject.FindGameObjectWithTag("PlayerRight");

		pcLeft = leftPlayer.GetComponent<PlayerControllerScript>();
		pcRight = rightPlayer.GetComponent<PlayerControllerScript>();

		if(pcLeft == null || pcRight == null){
			Debug.Log("Error: Couldn't find one or more players.");
		}

		// Find InputManager
		inputManager = GameObject.Find("InputManager").GetComponent<InputManagerScript>();
	}
	
	void Update () {

		PollInput();

		// Check that both are ready and give the input at same time (prevents desyncs)
		if(pcLeft.readyForInput && pcRight.readyForInput){
			pcLeft.GiveInputDirection(_inputDirection);
			pcRight.GiveInputDirection(_inputDirection);
		}

	}

	void PollInput(){

		// Direction stayed
		if(inputManager.GetDirectionStay(_inputDirection)){
			return;
		}

		_inputDirection = Direction.NONE;

		for(Direction d = Direction.NONE; d < Direction.NUM_DIRECTIONS; d++){
			if(inputManager.GetDirection(d)){
				_inputDirection = d;
			}
		}
	}
	
}
