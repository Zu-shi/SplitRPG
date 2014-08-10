using UnityEngine;
using System.Collections;

public class PlayerInputScript : MonoBehaviour {
	
	Direction _inputDirection;

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
				Globals.playerLeft.GiveInputDirection(_inputDirection);
				Globals.playerRight.GiveInputDirection(_inputDirection);
			}else{
				Globals.playerLeft.GiveInputAction();
				Globals.playerRight.GiveInputAction();
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
	}
	
}
