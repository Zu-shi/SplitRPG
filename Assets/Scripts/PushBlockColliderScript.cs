using UnityEngine;
using System.Collections;

public class PushBlockColliderScript : ColliderScript {
	
	MovementScript movementScript;

	void Start(){
		movementScript = GetComponent<MovementScript>();
	}

	public override bool TryToPush(GameObject pusher, Direction dir){
		return movementScript.MoveInDirection (dir);
	}

	public override bool CanPush(GameObject pusher, Direction dir){
		return movementScript.CanMoveInDirectionWithoutPushSideEffect(dir);
	}

}
