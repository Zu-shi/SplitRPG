using UnityEngine;
using System.Collections;

public class PushBlockColliderScript : ColliderScript {
	
	MovementScript movementScript;
	public AudioClip pushSound;
	public bool isHeavy = false;

	void Start(){
		movementScript = GetComponent<MovementScript>();
	}

	public virtual bool CanBePushedByPusher(MovementScript pusher){
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
		if (CanBePushedByPusher(pusher)) {
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
		if (CanBePushedByPusher(pusher)) {
			return movementScript.CanMoveInDirectionWithoutPushSideEffect (this.tileVector, dir);
		}else{
			return false;
		}
	}

}
