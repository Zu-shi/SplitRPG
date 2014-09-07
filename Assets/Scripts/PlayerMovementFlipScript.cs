using UnityEngine;
using System.Collections;

public class PlayerMovementFlipScript : _Mono {

	public 	bool flipX			= false;
	public 	bool flipY			= false;
	public 	bool affectLeft		= false;
	public 	bool affectRight	= false;

	private bool leftEntered	= false;
	private bool rightEntered	= false;

	void Update () {
		if(affectLeft && Globals.playerLeft.tileVector == this.tileVector) {
			if(!leftEntered) {
				leftEntered = true;
				Debug.Log("Doing flip.");
				if(flipX) 
					Globals.playerLeft.GetComponent<PlayerControllerScript>().FlipXMovement();
				if(flipY)
					Globals.playerLeft.GetComponent<PlayerControllerScript>().FlipYMovement();
			}
		} else {
			leftEntered = false;
		}
		if(affectRight && Globals.playerRight.tileVector == this.tileVector) {
			if(!rightEntered) {
				rightEntered = true;
				if(flipX) 
					Globals.playerRight.GetComponent<PlayerControllerScript>().FlipXMovement();
				if(flipY)
					Globals.playerRight.GetComponent<PlayerControllerScript>().FlipYMovement();
			}
		} else {
			rightEntered = false;
		}
	}
}
