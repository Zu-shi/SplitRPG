using UnityEngine;
using System.Collections;

public class PortalSenderScript : _Mono {

	[Tooltip("Where to send to character that steps on this portal.")]
	public PortalReceiverScript target;

	public bool allowLeftCharacter = true;
	public bool allowRightCharacter = true;


	public void OnTriggerStay2D(Collider2D other) {
		if(other.gameObject.tag == "PlayerLeft" && allowLeftCharacter) {
			if( !other.GetComponent<CharacterMovementScript>().isMoving ) {
				other.GetComponent<PlayerControllerScript>().tileX = target.tileX + (int)target.exitDirection.x;
				other.GetComponent<PlayerControllerScript>().tileY = target.tileY + (int)target.exitDirection.y;
			}
		}

		if(other.gameObject.tag == "PlayerRight" && allowRightCharacter) {
			if( !other.GetComponent<CharacterMovementScript>().isMoving ) {
				other.GetComponent<PlayerControllerScript>().tileX = target.tileX + (int)target.exitDirection.x;
				other.GetComponent<PlayerControllerScript>().tileY = target.tileY + (int)target.exitDirection.y;
			}
		}
	}

}
