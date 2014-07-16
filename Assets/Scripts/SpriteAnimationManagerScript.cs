using UnityEngine;
using System.Collections.Generic;

public class SpriteAnimationManagerScript : MonoBehaviour {

	private List<SpriteAnimationScript> animations;
	private SpriteAnimationScript currentAnim;
	private int currentFrame = 0;
	private float frameTimer = 0;
	private bool paused = false;

	public void Start() {
		animations = new List<SpriteAnimationScript>();
		SpriteAnimationScript[] anims = gameObject.GetComponents<SpriteAnimationScript>();
		foreach(SpriteAnimationScript anim in anims) {
			animations.Add(anim);
		}
		if(animations.Count > 0) {
			PlayAnimation(animations[0].animationName);
		}
	}

	public void PlayAnimation(string animationName = null, int startFrame = 0) {
		for(int i = 0; i < animations.Count; i++) {
			if(animations[i].animationName == animationName){
				currentAnim = animations[i];
				currentFrame = startFrame;
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
		if(frameTimer <= currentAnim.rate){
			return;
		} else {
			frameTimer = 0;
			currentFrame = (currentFrame + 1) % currentAnim.sprites.Length;
		}
	}
}
