using UnityEngine;
using System.Collections;

public class FaderScript : _Mono {

	float targetAlpha{get; set;}

	/// <summary>
	/// Gets or sets the fade rate.
	/// </summary>
	/// <value>The fade rate.</value>
	public float fadeRate {get; set;}

	public bool lockFade {get; set;}

	Utils.VoidDelegate fadeCallback;

	void Start () {
		guiAlpha = 1;
		lockFade = false;
		targetAlpha = guiAlpha;
		fadeRate = .01f;

		// Center on screen
		x = y = .5f;
	}
	
	void Update () {
		// Resize the gui element to fit the screen
		int w = Screen.width + 5;
		int h = Screen.height + 5;
		Rect r = new Rect(-w/2, -h/2, w, h);
		guiTexture.pixelInset = r;

		if(lockFade) {
			return;
		}
		// Fade towards the target alpha
		if(guiAlpha != targetAlpha){
			guiAlpha = fadeFunc(guiAlpha, targetAlpha, fadeRate);

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
	public void FadeDown(Utils.VoidDelegate callback = null){
		targetAlpha = 1;
		fadeCallback = callback;
	}

	/// <summary>
	/// Dims (partial fade down).
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void Dim(Utils.VoidDelegate callback = null){
		targetAlpha = .2f;
		fadeCallback = callback;
	}

	/// <summary>
	/// Fades up.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void FadeUp(Utils.VoidDelegate callback = null){
		targetAlpha = 0;
		fadeCallback = callback;
	}

	/// <summary>
	/// Helper function that makes the correct fading curve
	/// </summary>
	float fadeFunc(float value, float dest, float rate){
		// make the fade faster when closer to 1, otherwise it will look slow
		float bonus = Utils.Clamp(1 / (1 - value), 1, 40);
		value = Utils.MoveValueTowards(value, dest, rate * bonus);
		return value;
	}
}
