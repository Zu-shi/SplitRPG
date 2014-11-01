using UnityEngine;
using System.Collections;

public class FireExtinguisherScript1 : _Mono {

	public void Trigger() {
		if(Globals.fire1toggled)
			return;

		Globals.fire1toggled = true;
		Debug.Log ("Triggered");
		if(gameObject.layer == LayerMask.NameToLayer("Right"))
			GameObject.Find(Globals.levelManager.currentRightLevel).GetComponent<FireManagerScript>().PutOutFires();
		//else
		//	GameObject.Find(Globals.levelManager.currentLeftLevel).GetComponent<FireManagerScript>().PutOutFires();
	}
}
