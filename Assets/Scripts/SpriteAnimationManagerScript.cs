using UnityEngine;
using System.Collections.Generic;

public class SpriteAnimationManagerScript : MonoBehaviour {

	[Tooltip("If set, this sprite will revert to it's idle animation after it is told to play a one-shot animation.")]
	public bool idleAfterOneTimeAnimations = false;

	private List<SpriteAnimationScript> animations;
	private SpriteAnimationScript currentAnim;
	private SpriteRenderer sr;
	private int currentFrame = 0;
	private float frameTimer = 0;
	private bool paused = false;

	public void Start() {
		animations = new List<SpriteAnimationScript>();
		SpriteAnimationScript[] anims = gameObject.GetComponents<SpriteAnimationScript>();
		foreach(SpriteAnimationScript anim in anims) {
			animations.Add(anim);
		}
		sr = gameObject.GetComponent<SpriteRenderer>();
		if(animations.Count > 0) {
			PlayAnimation(animations[0].animationName);
		}
	}

	public void PlayAnimation(string animationName = null, int startFrame = 0) {
		if(animationName == null) {
			currentAnim = animations[0];
			currentFrame = startFrame;
			sr.sprite = currentAnim.sprites[currentFrame];
			return;
		}
		for(int i = 0; i < animations.Count; i++) {
			if(animations[i].animationName == animationName){
				currentAnim = animations[i];
				currentFrame = startFrame % currentAnim.sprites.Length;
				sr.sprite = currentAnim.sprites[currentFrame];
				return;
			}
		}

		Debug.LogError("No SpriteAnimationScript with name " + animationName + " found on GameObject named " + gameObject.name);
	}

	public void PauseAnimation(bool pauseAnimation = true){
		paused = pauseAnimation;
	}

	public void Update() {
		if(paused) {
			return;
		}
		if(currentAnim == null) {
			return;
		}
		frameTimer += Time.deltaTime;
		if(frameTimer <= 1.0f / currentAnim.rate){
			return;
		} else {
			frameTimer = 0;
			currentFrame = (currentFrame + 1) % currentAnim.sprites.Length;
			if(currentFrame == 0 && !currentAnim.loop) {
				if(idleAfterOneTimeAnimations){
					PlayAnimation(animations[0].animationName);
					return;
				}else {
					paused = true;
					return;
				}
			}

			sr.sprite = currentAnim.sprites[currentFrame];
		}
	}
}
