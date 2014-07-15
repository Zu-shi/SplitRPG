using UnityEngine;
using System.Collections;

public class FaderScript : _Mono {


	public float targetAlpha{get; set;}
	public float fadeRate{get;set;}
	
	void Start () {

		targetAlpha = 0;
		fadeRate = .01f;
	}
	
	void Update () {
		int w = Screen.width+5;
		int h = Screen.height+5;

		Rect r = new Rect(-w/2, -h/2, w, h);
		guiTexture.pixelInset = r;

		if(guiAlpha != targetAlpha){
			guiAlpha = Utils.MoveValueTowards(guiAlpha, targetAlpha, fadeRate);
//			Debug.Log(guiAlpha);
		}
	}
}
