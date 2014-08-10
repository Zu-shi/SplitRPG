using UnityEngine;
using System.Collections;

public class PlayerInputScript : MonoBehaviour {
	
	Direction _inputDirection;
	bool inputAction;

	/// <summary>
	/// The direction that is currently indicated by the player
	/// </summary>
	public Direction inputDirection {
		get {
			return _inputDirection;
		}
	}

	void Start () {
		_inputDirection = Direction.NONE;
	}
	
	void Update () {

		PollInput();

		// Check that both are ready and give the input at same time (prevents desyncs)
		if(Globals.playerLeft.readyForInput && Globals.playerRight.readyForInput){
			if(_inputDirection != Direction.NONE){
				//Debug.Log("Direction? = " + _inputDirection.ToString());
				Globals.playerLeft.GiveInputDirection(_inputDirection);
				Globals.playerRight.GiveInputDirection(_inputDirection);
			}
			else{
				//Debug.Log("Direction = NONE");
				if(inputAction){
					Debug.Log("Give Input Action Called.");
					Globals.playerLeft.GiveInputAction();
					Globals.playerRight.GiveInputAction();
				}
			}
		}

	}

	void PollInput(){

		// Direction stayed, in case another button is pressed, keep moving in same direction
		if(InputManager.GetDirectionStay(_inputDirection)){
			return;
		}

		_inputDirection = Direction.NONE;

		for(Direction d = Direction.NONE; d < Direction.NUM_DIRECTIONS; d++){
			if(InputManager.GetDirection(d)){
				_inputDirection = d;
			}
		}

		if (InputManager.GetButtonDown (Button.ACTION)) {
			inputAction = true;
		} else {
			inputAction = false;
		}
	}
}
