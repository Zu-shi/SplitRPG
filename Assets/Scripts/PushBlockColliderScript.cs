﻿using UnityEngine;
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
		if (CanBePushedByPusher(pusher)) {
			bool wasPushed = movementScript.MoveInDirection (dir);
			if (wasPushed) {
				Globals.soundManager.PlaySound (pushSound);
			}

			return wasPushed;
		} else {
			return false;
		}
	}

	public override bool CanPush(MovementScript pusher, Direction dir){
		if (CanBePushedByPusher(pusher)) {
			return movementScript.CanMoveInDirectionWithoutPushSideEffect (dir);
		}else{
			return false;
		}
	}

}
