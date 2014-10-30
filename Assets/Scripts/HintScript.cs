using UnityEngine;
using System.Collections;

public class HintScript : MonoBehaviour {

	private float r = 15;

	// Update is called once per frame
	void Update () {
		if(InputManager.GetButtonStay(Button.HINT)){
			GetComponent<ParticleSystem>().emissionRate = r; 
			//Debug.Log ("Showing hints");
		}else{
			GetComponent<ParticleSystem>().emissionRate = 0; 
		}
	}
}
