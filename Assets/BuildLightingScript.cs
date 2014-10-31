using UnityEngine;
using System.Collections;

public class BuildLightingScript : _Mono {

	public Sprite lighted;
	public Sprite unlighted;

	// Update is called once per frame
	void Update () {
		if(InputManager.GetButton(Button.HINT)){
			spriteRenderer.sprite = lighted;
		}else{
			spriteRenderer.sprite = unlighted;
		}
	}
}
