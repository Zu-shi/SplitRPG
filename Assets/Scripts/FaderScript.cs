using UnityEngine;
using System.Collections;

public class FaderScript : _Mono {

	float targetAlpha{get; set;}
	public float fadeRate{get;set;}

	Utils.VoidDelegate fadeCallback;

	void Start () {
		targetAlpha = guiAlpha;
		fadeRate = .01f;
	}
	
	void Update () {
		// Resize the gui element to fit the screen
		int w = Screen.width+5;
		int h = Screen.height+5;
		Rect r = new Rect(-w/2, -h/2, w, h);
		guiTexture.pixelInset = r;

		// Fade towards the target alpha
		if(guiAlpha != targetAlpha){
			guiAlpha = Utils.MoveValueTowards(guiAlpha, targetAlpha, fadeRate);

		} else {
			if(fadeCallback != null){
				fadeCallback();
				fadeCallback = null;
			}
		}
	}

	public void FadeDown(Utils.VoidDelegate callback){
		targetAlpha = 1;
		fadeCallback = callback;
	}

	public void Dim(Utils.VoidDelegate callback){
		targetAlpha = .2f;
		fadeCallback = callback;
	}

	public void FadeUp(Utils.VoidDelegate callback){
		targetAlpha = 0;
		fadeCallback = callback;
	}
}
