using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimationManagerScript : _Mono {

	[Tooltip("If set, this sprite will revert to it's idle animation after it is told to play a one-shot animation.")]
	public bool idleAfterOneTimeAnimations = false;
	[Tooltip("If set, the gameObject containing this animation will be destroyed after it is told to play a one-shot animation..")]
	public bool destroyAfterOneTimeAnimations = false;
	public bool playOnAwake = true;
	public string currentAnimationName{
		get{
			return currentAnim.animationName;
		}
	}

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

		if (animations.Count > 0) {
			PlayAnimation (animations [0].animationName);
			if(!playOnAwake){PauseAnimation();}
		}
	}

	public void PlayAnimation(string animationName = null, int startFrame = 0) {
		
		frameTimer = 0;
		paused = false;

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

	public void SetCurrentFrame( int frame ){
		frameTimer = 0;
		currentFrame = frame % currentAnim.sprites.Length;
		sr.sprite = currentAnim.sprites[currentFrame];
	}

	public void PauseAnimation(bool pauseAnimation = true){
		paused = pauseAnimation;
	}

	public void ResumeAnimation(){
		PauseAnimation (false);
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
				Debug.LogWarning("check destory animations");
				if(destroyAfterOneTimeAnimations){
					Destroy(gameObject);
				}else if(idleAfterOneTimeAnimations){
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
