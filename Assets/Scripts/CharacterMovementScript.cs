using UnityEngine;
using System.Collections;

public class CharacterMovementScript : MovementScript {

	public AudioClip walkingSound;


	protected override void StartMoving( Vector2 velocity){
		base.StartMoving (velocity);
		Globals.soundManager.PlaySound (walkingSound);
	}

}
