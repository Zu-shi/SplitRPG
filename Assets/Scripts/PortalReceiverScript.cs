using UnityEngine;
using System.Collections;

public class PortalReceiverScript : _Mono {

	[Tooltip("Where the character will land.")]
	public Direction characterTeleportDirection = Direction.NONE;

	private PortalSenderScript mySender;

	public void Start() {
		mySender = GetComponent<PortalSenderScript>();
 	}

	public void MovePlayerHere(PlayerControllerScript player) {
		if(mySender) {
			mySender.teleportDisabled = true;
		}
		player.tileVector = tileVector + exitDirection;
	}

	public Vector2 exitDirection {
		get {
			return 2 * Utils.DirectionToVector(characterTeleportDirection);
		}
	}
}
