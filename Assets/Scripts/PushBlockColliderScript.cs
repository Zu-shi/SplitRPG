using UnityEngine;
using System.Collections;

public class PushBlockColliderScript : ColliderScript {
	
	public override bool TryToPush(GameObject pusher, Direction dir){
		// See if we can be pushed in a direction
		Vector2 targetLoc = tileVector + 2 * Utils.DirectionToVector(dir);
		bool blocked = Globals.CollisionManager.TileBlocking(targetLoc);

		if(blocked){
			// Can't be pushed because we're blocked
			return false;

		} else {
			// Push ourselves to the new location
			tileVector = targetLoc;
			return true;
		}
	}

}
