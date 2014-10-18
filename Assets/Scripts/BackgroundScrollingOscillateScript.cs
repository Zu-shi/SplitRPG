using UnityEngine;
using System.Collections;

public class BackgroundScrollingOscillateScript : BackgroundScrollingTextureScript {
	
	public float min;
	public float max;
	public float duration;

	protected override void Update(){
		base.Update();
		alpha = sinOscillate(min, max, duration);
	}

}
