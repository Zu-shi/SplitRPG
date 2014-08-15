using UnityEngine;
using System.Collections;

public class PushBlockColliderScript : ColliderScript {
	
	MovementScript movementScript;
	public AudioClip pushSound;

	void Start(){
		movementScript = GetComponent<MovementScript>();
	}

	public override bool TryToPush(GameObject pusher, Direction dir){
		bool wasPushed = movementScript.MoveInDirection (dir);
		if (wasPushed) {
			Globals.soundManager.PlaySound(pushSound);
		}
		return wasPushed;
	}

	public override bool CanPush(GameObject pusher, Direction dir){
		return movementScript.CanMoveInDirectionWithoutPushSideEffect(dir);
	}

}
