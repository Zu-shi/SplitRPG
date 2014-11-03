using UnityEngine;
using System.Collections;

public class LatentHintScript : _Mono {

	private float amplitude = 0.5f;
	private float period = 2f;
	private float starty;
	private float location;
	private float counter;
	private float initialAlpha = 0.6f;
	private float alphaMultiplier = 0f;

	// Use this for initialization
	void Start () {
		starty = y;
		alpha = initialAlpha;
		counter = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		y = starty + amplitude * Mathf.Sin (counter);
		counter += Time.deltaTime * Mathf.PI * 2 / period;
		alpha = initialAlpha * alphaMultiplier;

		if(Globals.latentHintManager.countdownTimer < 0f){
			alphaMultiplier = Mathf.Clamp(alphaMultiplier + 0.05f, 0f, 1f);
		}else{
			alphaMultiplier = Mathf.Clamp(alphaMultiplier - 0.05f, 0f, 1f);
		}
	}
}
