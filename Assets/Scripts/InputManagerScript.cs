using UnityEngine;
using System.Collections.Generic;

public class InputManagerScript : MonoBehaviour {
	/*
	 * Probably don't use this script, use InputManager's static methods instead
	 */

	public bool ignoreInput;
	public bool snapshotShortcut = true;
	private const KeyCode SNAPSHOT_KEY = KeyCode.P;
	private KeyCode[] ACTION_KEYS = {KeyCode.Space, KeyCode.LeftControl, KeyCode.Z};

	List<Button> _inputs;
	List<Button> _lastInputs;

	
	void Start () {
		ignoreInput = false;
		_inputs = new List<Button>();
		_lastInputs = new List<Button>();
	}

	void LateUpdate () {
		// Update variables
		_lastInputs = _inputs;
		_inputs = new List<Button>();

		// Input
		if (snapshotShortcut) {
			if (Input.GetKeyDown (SNAPSHOT_KEY)) {
				string fname = "Assets/Snapshots/tmp/Screenshot" + System.DateTime.Now.ToString ("MM:DD hh:mm:ss.fff") + ".png";
				Debug.Log ("Snapshot saved: " + fname);
				Application.CaptureScreenshot (fname);
			}
		}

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

		foreach(KeyCode kc in ACTION_KEYS){
			if(Input.GetKeyDown(kc)){
				_inputs.Add(Button.ACTION);
				//Debug.LogWarning("added input");
			}
		}

		if(ignoreInput){
			_inputs.Clear();
		}

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
