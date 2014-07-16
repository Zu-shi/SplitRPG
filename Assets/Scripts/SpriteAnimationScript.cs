using UnityEngine;
using System.Collections;

// A simple data carrier. Used by SpriteAnimationManager components.
public class SpriteAnimationScript : _Mono {

	public string animationName = "Animation";
	public float rate = 10; // Frames per second
	public bool loop = true;
	public Sprite[] sprites;
}
