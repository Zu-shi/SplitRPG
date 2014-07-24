using UnityEngine;
using System.Collections.Generic;

public class InputManagerScript : MonoBehaviour {

	List<Button> _inputs;
	List<Button> _lastInputs;


	void Start () {
		_inputs = new List<Button>();
		_lastInputs = new List<Button>();
	}
	
	void LateUpdate () {
		// Update variables
		_lastInputs = _inputs;
		_inputs = new List<Button>();

		// Input
		float hIn = Input.GetAxis("Horizontal");
		float vIn = Input.GetAxis("Vertical");
		
		// How far does the stick have to be pressed to be considered a direction
		float threshhold = .5f;

		// Horizontal/Vertical directions of input
		if(hIn > threshhold)
			_inputs.Add(Button.RIGHT);
		else if (hIn < -threshhold)
			_inputs.Add(Button.LEFT);

		if(vIn > threshhold)
			_inputs.Add(Button.UP);
		else if (vIn < -threshhold)
			_inputs.Add(Button.DOWN);

	}

	Button DirectionToButton(Direction d){
		switch(d){
		case Direction.UP:
			return Button.UP;
		case Direction.LEFT:
			return Button.LEFT;
		case Direction.RIGHT:
			return Button.RIGHT;
		case Direction.DOWN:
			return Button.DOWN;
		default:
			return Button.NONE;
		}
	}

	public bool GetButton(Button button){
		if(_inputs.Contains(button))
			return true;
		else 
			return false;
	}

	public bool GetButtonDown(Button button){
		if(_inputs.Contains(button) && !_lastInputs.Contains(button))
			return true;
		else 
			return false;
	}

	public bool GetButtonStay(Button button){
		if(_inputs.Contains(button) && _lastInputs.Contains(button))
			return true;
		else 
			return false;
	}

	public bool GetDirection(Direction direction){
		Button button = DirectionToButton(direction);
		if(_inputs.Contains(button))
			return true;
		else 
			return false;
	}
	
	public bool GetDirectionDown(Direction direction){
		Button button = DirectionToButton(direction);
		if(_inputs.Contains(button) && !_lastInputs.Contains(button))
			return true;
		else 
			return false;
	}
	
	public bool GetDirectionStay(Direction direction){
		Button button = DirectionToButton(direction);
		if(_inputs.Contains(button) && _lastInputs.Contains(button))
			return true;
		else 
			return false;
	}
}
