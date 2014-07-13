using UnityEngine;
using System.Collections;

public class PlayerInputScript : MonoBehaviour {


	Direction _inputDirection;


	public Direction inputDirection {
		get {
			return _inputDirection;
		}
	}


	void Start () {
		_inputDirection = Direction.NONE;
	}
	
	void Update () {

		// Input
		float hIn = Input.GetAxis("Horizontal");
		float vIn = Input.GetAxis("Vertical");

		// How far does the stick have to be pressed to be considered a direction
		float threshhold = .5f;

		// Horizontal/Vertical directions of input
		int hDirIn = 0;
		int vDirIn = 0;

		if(hIn > threshhold)
			hDirIn = 1;
		else if (hIn < -threshhold)
			hDirIn = -1;

		if(vIn > threshhold)
			vDirIn = 1;
		else if (vIn < -threshhold)
			vDirIn = -1;


		// Current Horizontal/Vertical directions
		int hDirCurr = 0;
		int vDirCurr = 0;

		switch(_inputDirection){
		case Direction.LEFT:
			hDirCurr = -1;
			break;
		case Direction.RIGHT:
			hDirCurr = 1;
			break;
		case Direction.UP:
			vDirCurr = 1;
			break;
		case Direction.DOWN:
			vDirCurr = -1;
			break;
		}

		// Final directions, taking into account current direction and inputs
		int hDir = 0;
		int vDir = 0;

		bool wasMovingHorizontally = (hDirCurr != 0);
		bool wasMovingVertically = (vDirCurr != 0);

		if(wasMovingHorizontally){
			if(hDirIn != 0){
				hDir = hDirIn;
			} else {
				vDir = vDirIn;
			}

		} else if (wasMovingVertically){
			if(vDirIn != 0){
				vDir = vDirIn;
			} else {
				hDir = hDirIn;
			}

		} else {
			if(hDirIn != 0)
				hDir = hDirIn;
			else
				vDir = vDirIn;
		}

		if(hDir != 0 && vDir != 0){
			Debug.Log("ERROR: Input system error.");
		}



		// Set final input direction
		if(hDir > 0)
			_inputDirection = Direction.RIGHT;
		else if(hDir < 0)
			_inputDirection = Direction.LEFT;
		else if(vDir < 0)
			_inputDirection = Direction.DOWN;
		else if(vDir > 0)
			_inputDirection = Direction.UP;
		else
			_inputDirection = Direction.NONE;


		// Debugging
//		Debug.Log("" + hDirIn + ", " + vDirIn);
//		Debug.Log(inputDirection);

	}
}
