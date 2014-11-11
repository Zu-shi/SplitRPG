using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class LineScript : _Mono {
	
	private float amplitude = 2f;
	private float period = 3f;
	private float startxs;
	private float counter;
	private bool fadeDown = false;

	void Start () {
		startxs = xs;
	}

	public void LineVisible(bool visible = true) {
		spriteRenderer.enabled = visible;
	}

	public void Update() {
		//if(Globals.levelManager.currentLeftLevel == "SanctuaryLeft_NewTileset")
		//	spriteRenderer.enabled = false;
		if(!Globals.OptionsManager.paused){
			xs = startxs + amplitude * Mathf.Sin (counter);
			counter += Time.deltaTime * Mathf.PI * 2 / period;
		}

		if(fadeDown){
			alpha -= 0.01f;
			if(alpha == 0f){
				alpha = 1f;
				fadeDown =false;
				LineVisible(false);
			}
		}
	}

	public void FadeDown() {
		fadeDown = true;
	}
}
