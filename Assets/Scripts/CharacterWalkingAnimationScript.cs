using UnityEngine;
using System.Collections;

/// <summary>
/// Handles animating the player characters in coordination with their given inputs.
/// </summary>
/// <author>Zuoming Shi</author>
[RequireComponent(typeof(SpriteAnimationManagerScript))]
public class CharacterWalkingAnimationScript : MonoBehaviour {

	private CharacterMovementScript mov;
	private SpriteAnimationManagerScript sprAnimMan;
	private bool playingAnimation;
	private const float timeForWalkingAnimationToEnd = 0.04f;
	private float waitedTime = 0f;

	void Start () {

		mov = gameObject.GetComponentInParent<CharacterMovementScript> ();
		if (mov == null) {
			// This error message must be logged since RequireComponent cannot garuntee its presence.
			Debug.LogError("Error: CharacterWalkingAnimationScript is not attatched to child of an object with movement script");		
		}

		sprAnimMan = gameObject.GetComponent<SpriteAnimationManagerScript> ();
	}

	// Update is called once per frame
	void Update () {
		if(sprAnimMan.currentAnimationName.Contains("Walk")) {
			updateWalkingState ();
		}
	}

	/// <summary>
	/// Control command that updates walking state of the character.
	/// </summary>
	private void updateWalkingState(){
		if (mov.isMoving && !playingAnimation) {
			StartWalkingAnimation ();
		} else if (!mov.isMoving && playingAnimation) { 
			if (waitedTime < timeForWalkingAnimationToEnd) {
				waitedTime += Time.deltaTime;
			} else {
				StopWalkingAnimation ();
			}
		} else if (mov.isMoving && playingAnimation) {
			AccountForDirectionChange();
		} else if (mov.isChangingDirection){
			AccountForDirectionChange();
			StopWalkingAnimation ();
		}
	}

	/// <summary>
	/// If the player's direction changes while walking, play new animation
	/// </summary>
	private void AccountForDirectionChange(){
		if( getDirectionToAnimationName() !=  sprAnimMan.currentAnimationName){
			sprAnimMan.PlayAnimation(getDirectionToAnimationName(), 0);
//			Debug.Log(getDirectionToAnimationName() + " " + sprAnimMan.currentAnimationName);
		}
	}
	
	/// <summary>
	/// Sets the walking animation according to direction
	/// </summary>
	private void StartWalkingAnimation(){
		waitedTime = 0f;

		string animationName = getDirectionToAnimationName();
		
		if(animationName == ""){
			Debug.LogError("Character is moving, but direction is not down, up, left, or right.");
		}
		
		if(sprAnimMan.currentAnimationName == animationName){
			sprAnimMan.ResumeAnimation(); //If the animation is already the same as the current one, continue current animation.
		}else{
			sprAnimMan.PlayAnimation(animationName, 0); //Otherwise, start a new animation at frame 0 (idle);
		}
		
		playingAnimation = true;
	}

	/// <summary>
	/// Stops the walking animation and set current animation to idle.
	/// </summary>
	private void StopWalkingAnimation(){
		sprAnimMan.PauseAnimation();
		sprAnimMan.SetCurrentFrame(0); //0 is the idle animation.
		playingAnimation = false;
//		Debug.Log("Animation Stopped");
	}

	/// <summary>
	/// Convert direction to name of animation
	/// </summary>
	private string getDirectionToAnimationName(){
		string result = "";
		switch(mov.moveDirection){
			case(Direction.DOWN): {result = "WalkDownAnimation"; break;}
			case(Direction.UP): {result  = "WalkUpAnimation"; break;}
			case(Direction.LEFT): {result = "WalkLeftAnimation"; break;}
			case(Direction.RIGHT): {result = "WalkRightAnimation"; break;}
			default: {result  = "WalkUpAnimation"; break;}
		}
		return result;
	}
}
