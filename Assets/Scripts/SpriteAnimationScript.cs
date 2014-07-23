using UnityEngine;
using System.Collections;

// A simple data carrier. Used by SpriteAnimationManager components.
public class SpriteAnimationScript : _Mono {

	[Tooltip("The name that is used to reference this animation, i.e. idle, walking, etc.")]
	public string animationName = "Animation";

	[Tooltip("The rate, in frames per second, at which this animation should be played.")]
	public float rate = 10;

	[Tooltip("If set, this animation will loop when it is played.")]
	public bool loop = true;

	[Tooltip("The sequence of sprites that make up this animation.")]
	public Sprite[] sprites;
}
