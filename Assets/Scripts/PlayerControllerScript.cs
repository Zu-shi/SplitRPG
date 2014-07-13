using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {

	PlayerInputScript playerInput;
	CharacterMovementScript characterMovement;

	void Start () {
		playerInput = GetComponent<PlayerInputScript>();
		if(playerInput == null){
			Debug.Log ("Error: PlayerInputScript not found.");
		}

		characterMovement = GetComponent<CharacterMovementScript>();
		if(characterMovement == null){
			Debug.Log ("Error: CharacterMovementScript not found.");
		}
	}
	
	void Update () {
		characterMovement.MoveInDirection(playerInput.inputDirection);
	}

}
