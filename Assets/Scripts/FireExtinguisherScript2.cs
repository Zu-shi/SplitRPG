using UnityEngine;
using System.Collections;

public class FireExtinguisherScript2 : _Mono {
	
	public void Trigger() {
		if(Globals.fire2toggled)
			return;
		
		Globals.fire2toggled = true;
		Debug.Log ("Triggered");
		if(gameObject.layer == LayerMask.NameToLayer("Right"))
			GameObject.Find(Globals.levelManager.currentRightLevel).GetComponent<FireManagerScript>().PutOutFires();
		//else
		//	GameObject.Find(Globals.levelManager.currentLeftLevel).GetComponent<FireManagerScript>().PutOutFires();
	}
}
