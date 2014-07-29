using UnityEngine;
using System.Collections;

public class PortalReceiverScript : _Mono {

	[Tooltip("Where the character will land.")]
	public Direction characterTeleportDirection = Direction.NONE;

	public Vector2 exitDirection {
		get {
			return 2 * Utils.DirectionToVector(characterTeleportDirection);
		}
	}
}
