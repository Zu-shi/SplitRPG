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

	/// <summary>
	/// Is the button currenty pressed
	/// </summary>
	public static bool GetButton(Button button){
		return Instance.GetButton(button);
	}

	/// <summary>
	/// Was the button pressed this frame
	/// </summary>
	public static bool GetButtonDown(Button button){
		return Instance.GetButtonDown(button);
	}

	/// <summary>
	/// Is the button held down from a previous frame
	/// </summary>
	public static bool GetButtonStay(Button button){
		return Instance.GetButtonStay(button);
	}

	/// <summary>
	/// Is the direction currenty pressed
	/// </summary>
	public static bool GetDirection(Direction direction){
		return Instance.GetDirection(direction);
	}

	/// <summary>
	/// Was the direction pressed this frame
	/// </summary>
	public static bool GetDirectionDown(Direction direction){
		return Instance.GetDirectionDown(direction);
	}

	/// <summary>
	/// Is the direction held down from a previous frame
	/// </summary>
	public static bool GetDirectionStay(Direction direction){
		return Instance.GetDirectionStay(direction);
	}
}
