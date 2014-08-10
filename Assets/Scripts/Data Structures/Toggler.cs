using UnityEngine;
using System.Collections;

/// <summary>
/// A fancy type of bool.
/// </summary>
/// <author>Mark Gardner</author>
public class Toggler {

	private bool _on = true;

	/// <summary>
	/// Gets a value indicating whether this <see cref="Switch"/> is on.
	/// </summary>
	/// <value><c>true</c> if on; otherwise, <c>false</c>.</value>
	public bool on {
		get { return _on; }
	}
	/// <summary>
	/// Gets a value indicating whether this <see cref="Switch"/> is off.
	/// </summary>
	/// <value><c>true</c> if off; otherwise, <c>false</c>.</value>
	public bool off {
		get { return !_on; }
	}

	public Toggler(){
		_on = false;
	}

	/// <summary>
	/// Turns the switch on.
	/// </summary>
	public void TurnOn(){
		_on = true;
	}

	/// <summary>
	/// Turns the switch off.
	/// </summary>
	public void TurnOff(){
		_on = false;
	}

	/// <summary>
	/// Toggle the switch.
	/// </summary>
	public void Toggle(){
		_on = !_on;
	}

}
