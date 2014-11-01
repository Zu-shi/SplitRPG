using UnityEngine;
using System.Collections;

public class FireExtinguisherScript3 : _Mono {
	
	public void Trigger() {
		if(Globals.fire3toggled)
			return;
		
		Globals.fire3toggled = true;
		Debug.Log ("Triggered");
		if(gameObject.layer == LayerMask.NameToLayer("Right"))
			GameObject.Find(Globals.levelManager.currentRightLevel).GetComponent<FireManagerScript>().PutOutFires();
		//else
		//	GameObject.Find(Globals.levelManager.currentLeftLevel).GetComponent<FireManagerScript>().PutOutFires();
	}
}
