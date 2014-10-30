using UnityEngine;
using System.Collections;

public class SplashScript : _Mono {
	
	public AudioClip splashSound;

	// Use this for initialization
	void Start () {
		alpha = 0.8f;
		Globals.soundManager.PlaySound(splashSound);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
