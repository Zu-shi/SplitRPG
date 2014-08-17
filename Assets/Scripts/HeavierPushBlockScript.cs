using UnityEngine;
using System.Collections;

public class HeavierPushBlockScript : PushBlockColliderScript {

	public virtual bool CanBePushedByPusher(MovementScript pusher){
		return (pusher.canPush && pusher.canPushHeavy);
	}

}
