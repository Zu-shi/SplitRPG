using UnityEngine;
using System.Collections;

public class PlayerControllerScript : _Mono {

	// Other player stuff
	bool isLeftPlayer;
	GameObject otherPlayer;
	PlayerControllerScript otherPlayerController;
	
	// Other scripts
	CharacterMovementScript characterMovement;
	FallingBehaviorScript fallingBehavior;

	// Spawn point
	int spawnX, spawnY;


	public bool readyForInput {
		get {
			return !characterMovement.isMoving && !fallingBehavior.falling;
		}
	}

	void Start () {
		// Determine which player we are
		if(gameObject.tag == "PlayerLeft"){
			isLeftPlayer = true;
		} else {
			isLeftPlayer = false;
		}

		// Get other player
		if(isLeftPlayer){
			otherPlayer = GameObject.FindGameObjectWithTag("PlayerRight");
		} else {
			otherPlayer = GameObject.FindGameObjectWithTag("PlayerLeft");
		}
		if(otherPlayer == null){
			Debug.Log ("Error: Other player not found.");
		}

		// Get other player controller
		otherPlayerController = otherPlayer.GetComponent<PlayerControllerScript>();
		if(otherPlayerController == null){
			Debug.Log ("Error: Other player controller not found.");
		}

		// Get scripts
		characterMovement = GetComponent<CharacterMovementScript>();
		if(characterMovement == null){
			Debug.Log ("Error: CharacterMovementScript not found.");
		}
		fallingBehavior = GetComponent<FallingBehaviorScript>();
		if(fallingBehavior == null){
			Debug.Log ("Error: FallingBehaviorScript not found.");
		}


		// Set spawn point
		spawnX = tileX;
		spawnY = tileY;

	}

	public void ResetPlayer(){

		// Move to spawn point
		x = spawnX;
		y = spawnY;

		fallingBehavior.Reset();

	}

	void ResetBothPlayers(){
		ResetPlayer();
		otherPlayerController.ResetPlayer();
	}
	
	void Update () {

		// Check if fell
		if(fallingBehavior.fell){
			ResetBothPlayers();
			return;
		}

	}

	public void GiveInputDirection(Direction direction){
		if(!fallingBehavior.falling){
			characterMovement.MoveInDirection(direction);
		}
	}

}
