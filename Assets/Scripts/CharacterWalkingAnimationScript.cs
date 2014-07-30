using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteAnimationManagerScript))]
public class CharacterWalkingAnimationScript : MonoBehaviour {

	private MovementScript mov;
	private SpriteAnimationManagerScript sprAnimMan;
	private bool playingAnimation;
	private const float timeForWalkingAnimationToEnd = 0.04f;
	private float waitedTime = 0f;

	void Start () {

		mov = gameObject.GetComponentInParent<MovementScript> ();
		if (mov == null) {
			// This error message must be logged since RequireComponent cannot garuntee its presence.
			Debug.LogError("Error: CharacterWalkingAnimationScript is not attatched to child of an object with movement script");		
		}

		sprAnimMan = gameObject.GetComponent<SpriteAnimationManagerScript> ();
	}

	// Update is called once per frame
	void Update () {
		updateWalkingState ();
	}

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
		}
	}

	private void AccountForDirectionChange(){
		if( getDirectionToAnimationName() !=  sprAnimMan.currentAnimationName){
			sprAnimMan.PlayAnimation(getDirectionToAnimationName(), 0);
			Debug.Log(getDirectionToAnimationName() + " " + sprAnimMan.currentAnimationName);
		}
	}

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

	private void StopWalkingAnimation(){
		sprAnimMan.PauseAnimation();
		sprAnimMan.SetCurrentFrame(0); //0 is the idle animation.
		playingAnimation = false;
		Debug.Log("Animation Stopped");
	}

	private string getDirectionToAnimationName(){
		string result = "";
		switch(mov.moveDirection){
			case(Direction.DOWN): {result = "WalkDownAnimation"; break;}
			case(Direction.UP): {result  = "WalkUpAnimation"; break;}
			case(Direction.LEFT): {result = "WalkLeftAnimation"; break;}
			case(Direction.RIGHT): {result = "WalkRightAnimation"; break;}
		}
		return result;
	}
}
