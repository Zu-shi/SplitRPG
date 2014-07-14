using UnityEngine;
using System.Collections;

public class PlayerControllerScript : _Mono {

	public bool allowMovement{get;set;}

	// Game Manager
	GameManagerScript gameManager;

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
		allowMovement = true;

		// Game Manager
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
		if(gameManager == null){
			Debug.Log ("Error: Game Manager not found.");
		}

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

		// Reset Camera (currently a hack)
		gameManager.Reset();
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

	public bool WillMoveOffScreen(Direction direction){

		switch(direction){
		case Direction.NONE:
			break;
		case Direction.LEFT:
			if(tileX == gameManager.roomLeft)
				return true;
			break;
		case Direction.RIGHT:
			if(tileX == gameManager.roomRight)
				return true;
			break;
		case Direction.UP:
			if(tileY == gameManager.roomTop)
				return true;
			break;
		case Direction.DOWN:
			if(tileY == gameManager.roomBot)
				return true;
			break;

		}


		return false;
	}

	public void GiveInputDirection(Direction direction){
		if(fallingBehavior.falling || !allowMovement)
			return;

		if(WillMoveOffScreen(direction)){
			if(otherPlayerController.WillMoveOffScreen(direction)){

				gameManager.MoveScreen(direction);

			} else {
				return;
			}
		}

		characterMovement.MoveInDirection(direction);
	}

	public void EnableMovement(){
		allowMovement = true;
	}

	public void DisableMovement(float t){
		allowMovement = false;
		CancelInvoke("EnableMovement");
		Invoke ("EnableMovement", t);
	}

}
