using UnityEngine;
using System.Collections;

public class HintScript : _Mono {

	private float r = 15f;
	private float spritefaderade = 0.2f;
	public Sprite hintLeft;
	public Sprite hintRight;

	void Start(){
		alpha = 0;
		GetComponent<ParticleSystem>().emissionRate = 0;
		if(gameObject.layer == LayerMask.NameToLayer("Left")){
			spriteRenderer.sprite = hintLeft;
		}
		if(gameObject.layer == LayerMask.NameToLayer("Right")){
			spriteRenderer.sprite = hintRight;
		}
	}

	// Update is called once per frame
	void Update () {
		if(InputManager.GetButtonStay(Button.HINT)){
			GetComponent<ParticleSystem>().emissionRate = r; 
			alpha = Utils.Clamp(alpha + spritefaderade, 0f, 0.8f);
			//Debug.Log ("Showing hints");
		}else{
			GetComponent<ParticleSystem>().emissionRate = 0; 
			alpha = Utils.Clamp(alpha - spritefaderade, 0f, 0.8f);
		}
	}
}
