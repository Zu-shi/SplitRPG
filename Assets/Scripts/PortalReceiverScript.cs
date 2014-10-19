using UnityEngine;
using System.Collections;

public class PortalReceiverScript : _Mono {

	[Tooltip("Where the character will land.")]
	public Direction characterTeleportDirection = Direction.NONE;

	private PortalSenderScript mySender;
	private bool fadeTransition = false;

	public void Start() {
		mySender = GetComponent<PortalSenderScript>();
 	}

	public void MovePlayerHere(PlayerControllerScript player, bool fadeTransition = false) {
		if(mySender) {
			mySender.teleportDisabled = true;
		}
		if(fadeTransition) {
			Globals.roomManager.fadeTransition = true;
			this.fadeTransition = fadeTransition;
		}
		player.tileVector = tileVector + exitDirection;
	}

	public void OnTriggerExit2D(Collider2D other) {
		if(fadeTransition){
			Globals.roomManager.fadeTransition = false;
		}
	}


	public Vector2 exitDirection {
		get {
			return 2 * Utils.DirectionToVector(characterTeleportDirection);
		}
	}
}
