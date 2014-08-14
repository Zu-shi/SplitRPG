using UnityEngine;
using System.Collections;

/// <summary>
/// Movement specific to a player character, attatched to player objects. 
/// This isn't really specific to the player yet, the Movement script stuff should be refactored here.
/// </summary>
/// <author>Mark Gardner</author>
public class CharacterMovementScript : MovementScript {

	public AudioClip walkingSound;
	public AudioClip wrongBeep;
	private const float wrongBeatWaitTime = 1.0f;

	protected override void StartMoving( Vector2 velocity){
		base.StartMoving (velocity);
		Globals.soundManager.PlaySound (walkingSound);
		//return yield (WaitForSeconds (1.0f));
	}

	protected void PlayWrongBeep(){
		Globals.soundManager.PlaySound (wrongBeep);
	}

	protected override void StartFall(){
		base.StartFall ();
		Invoke("PlayWrongBeep", 0.6f);
	}

	/// <summary>
	/// This method pretends that the player is moving in order to sync the characters when one is against a wall while the other is at the exit.
	/// </summary>
	/// <param name="direction">Direction.</param>
	public bool PretendMoveInDirection(Direction direction){
		if(_isMoving || _isChangingDirection || direction == Direction.NONE){
			return false;
		}
		
		moveDirection = direction;
		_isMoving = true;
		moveTimeLeft = moveTime;
		StartMoving(Utils.DirectionToVector(direction) * 0.0f);
		return true;
	}

}
