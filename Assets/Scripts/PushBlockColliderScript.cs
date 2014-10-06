using UnityEngine;
using System.Collections;

public class PushBlockColliderScript : ColliderScript {
	
	MovementScript movementScript;
	public AudioClip pushSound;
	public bool isHeavy = false;

	void Start(){
		movementScript = GetComponent<MovementScript>();
	}

	public virtual bool CanBePushedByPusher(MovementScript pusher, Direction dir){
		Room room = Globals.roomManager.GetRoom(pusher.gameObject.layer);
		//Disallow a block from being pushed outside of a room
		if (!room.ContainsTile (this.tileVector + Utils.DirectionToVector (dir) * 2)) {
			return false;
		}

		if (isHeavy) {
			if(!pusher.canPush & pusher.canPushHeavy){
				Debug.LogWarning("Warning: MovementScript canPushHeavy set to true but canPush set to false.");
			}
			return pusher.canPush & pusher.canPushHeavy;
		} else {
			return pusher.canPush;
		}
	}

	public override bool TryToPush(MovementScript pusher, Direction dir){
		//Debug.Log ("Trying to push");
		if (CanBePushedByPusher(pusher, dir)) {
			bool wasPushed = movementScript.MoveInDirection (dir);
			//Debug.Log ("Trying to psuh returned " + wasPushed);
			if (wasPushed) {
				Globals.soundManager.PlaySound (pushSound);
				//Debug.Log ("Sound played");
			}

			return wasPushed;
		} else {
			return false;
		}
	}

	public override bool CanPush(MovementScript pusher, Direction dir){
		//Debug.Log("Canpush");
		if (CanBePushedByPusher(pusher, dir)) {
			//Debug.Log ("Can be pushed by pusher");
			Room room = Globals.roomManager.GetRoom(gameObject.layer);
			//Debug.Log ("Verdict: " + movementScript.CanMoveInDirectionWithoutPushSideEffect (this.tileVector, dir) && room.ContainsTile(this.tileVector + Utils.DirectionToVector(dir) * 2));
			return movementScript.CanMoveInDirectionWithoutPushSideEffect (this.tileVector, dir) && room.ContainsTile(this.tileVector + Utils.DirectionToVector(dir) * 2);
		}else{
			return false;
		}
	}

}
