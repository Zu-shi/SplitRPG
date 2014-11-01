using UnityEngine;
using System.Collections;

public class HintScript : _Mono {

	private float r = 15;
	private float spritefaderade = 0.2f;

	void Start(){
		alpha = 0;
		GetComponent<ParticleSystem>().emissionRate = 0; 
	}

	// Update is called once per frame
	void Update () {
		if(InputManager.GetButtonStay(Button.HINT)){
			GetComponent<ParticleSystem>().emissionRate = r; 
			alpha = Utils.Clamp(alpha + spritefaderade, 0, 1);
			//Debug.Log ("Showing hints");
		}else{
			GetComponent<ParticleSystem>().emissionRate = 0; 
			alpha = Utils.Clamp(alpha - spritefaderade, 0, 1);
		}
	}
}
