using UnityEngine;
using System.Collections;

/// <summary>
/// Movement specific to a player character, attatched to player objects.
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
}
