using UnityEngine;
using System.Collections;

public class PortalSenderScript : _Mono {

	[Tooltip("Where to send to character that steps on this portal.")]
	public PortalReceiverScript target;

	// This is set from an attached receiver script, if we have one. This allows us to avoid infinite
	// teleport loops between two bidirectional portals.
	public bool teleportDisabled = false;
	public bool fadeTransition = false;

	public void Update() {
		if(Globals.collisionManager.IsPlayerOnTile(tileVector, gameObject.layer)) {			// A player is on us
			if(teleportDisabled) return;													// We shouldn't be teleporting

			if(Globals.playerLeft.gameObject.layer == this.gameObject.layer) {				// Left player is on us
				//if(Globals.playerLeft.GetComponent<MovementScript>().isMoving) return;		// Don't teleport until they stop moving
				target.MovePlayerHere(Globals.playerLeft, fadeTransition);
			}
			else {																			// Right player is on us
				if(Globals.playerRight.GetComponent<MovementScript>().isMoving) return;		// Don't teleport until they stop moving
				target.MovePlayerHere(Globals.playerRight, fadeTransition);
			}
		}
		else {
			if(teleportDisabled) {
				teleportDisabled = false;
			}
		}
	}
}
