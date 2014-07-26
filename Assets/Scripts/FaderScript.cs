using UnityEngine;
using System.Collections;

public class FaderScript : _Mono {

	float targetAlpha{get; set;}

	/// <summary>
	/// Gets or sets the fade rate.
	/// </summary>
	/// <value>The fade rate.</value>
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
				Utils.VoidDelegate tmp = fadeCallback;
				fadeCallback = null;
				tmp();
			}
		}
	}

	/// <summary>
	/// Fades down.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void FadeDown(Utils.VoidDelegate callback){
		targetAlpha = 1;
		fadeCallback = callback;
	}

	/// <summary>
	/// Dims (partial fade down).
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void Dim(Utils.VoidDelegate callback){
		targetAlpha = .2f;
		fadeCallback = callback;
	}

	/// <summary>
	/// Fades up.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void FadeUp(Utils.VoidDelegate callback){
		targetAlpha = 0;
		fadeCallback = callback;
	}
}
