using UnityEngine;
using System.Collections;

public class Switch {

	bool _on;
	public bool on {
		get { return _on; }
	}
	public bool off {
		get { return !_on; }
	}

	public Switch(){
		_on = false;
	}

	public void TurnOn(){
		_on = true;
	}

	public void TurnOff(){
		_on = false;
	}

	public void Toggle(){
		_on = !_on;
	}

}
