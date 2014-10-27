using UnityEngine;
using System.Collections;

public class FireExtinguisherScript : _Mono {

	public bool triggered = false;

	public void Trigger() {
		if(triggered)
			return;

		triggered = true;
		if(gameObject.layer == LayerMask.NameToLayer("Right"))
			GameObject.Find(Globals.levelManager.currentRightLevel).GetComponent<FireManagerScript>().PutOutFires();
		else
			GameObject.Find(Globals.levelManager.currentLeftLevel).GetComponent<FireManagerScript>().PutOutFires();
	}
}
