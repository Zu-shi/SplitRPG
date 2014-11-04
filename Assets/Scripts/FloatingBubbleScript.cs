using UnityEngine;
using System.Collections;

public class FloatingBubbleScript : _Mono {

	private float amplitude = 0.5f;
    private float period = 2f;
	private float starty;
	private float location;
	private float counter;

	// Use this for initialization
	void Start () {
		starty = y;
		alpha = 0.7f;
		counter = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(!Globals.OptionsManager.paused){
			y = starty + amplitude * Mathf.Sin (counter);
			counter += Time.deltaTime * Mathf.PI * 2 / period;
		}
	}
}
