using UnityEngine;
using System.Collections;

public class InputManager {

	static InputManagerScript _inputManager = null;

	static void FindInputManager(){
		if(_inputManager == null)
			_inputManager = GameObject.Find("InputManager").GetComponent<InputManagerScript>();
	}

	public static bool GetButton(Button button){
		FindInputManager();
		return _inputManager.GetButton(button);
	}
	
	public static bool GetButtonDown(Button button){
		FindInputManager();
		return _inputManager.GetButtonDown(button);
	}
	
	public static bool GetButtonStay(Button button){
		FindInputManager();
		return _inputManager.GetButtonStay(button);
	}
	
	public static bool GetDirection(Direction direction){
		FindInputManager();
		return _inputManager.GetDirection(direction);
	}
	
	public static bool GetDirectionDown(Direction direction){
		FindInputManager();
		return _inputManager.GetDirectionDown(direction);
	}
	
	public static bool GetDirectionStay(Direction direction){
		FindInputManager();
		return _inputManager.GetDirectionStay(direction);
	}
}
