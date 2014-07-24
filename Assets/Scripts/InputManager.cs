using UnityEngine;
using System.Collections;

public class InputManager {

	static InputManagerScript _inputManager = null;

	public static InputManagerScript Instance{
		get{
			if(_inputManager == null)
				_inputManager = GameObject.Find("InputManager").GetComponent<InputManagerScript>();
			return _inputManager;
		}
	}


	public static bool GetButton(Button button){
		return Instance.GetButton(button);
	}
	
	public static bool GetButtonDown(Button button){
		return Instance.GetButtonDown(button);
	}
	
	public static bool GetButtonStay(Button button){
		return Instance.GetButtonStay(button);
	}
	
	public static bool GetDirection(Direction direction){
		return Instance.GetDirection(direction);
	}
	
	public static bool GetDirectionDown(Direction direction){
		return Instance.GetDirectionDown(direction);
	}
	
	public static bool GetDirectionStay(Direction direction){
		return Instance.GetDirectionStay(direction);
	}
}
