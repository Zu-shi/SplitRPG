using UnityEngine;
using System.Collections;

public class PlayerControllerScript : _Mono {

	public bool allowMovement{get;set;}

	[Tooltip("Shadow object of the player")]
	public GameObject shadow;

	// Room Manager
	RoomManagerScript roomManager;

	// Other player stuff
	bool isLeftPlayer;
	PlayerControllerScript otherPlayer;

	// Other scripts
	CharacterMovementScript characterMovement;
	FallingBehaviorScript fallingBehavior;

	// Spawn point
	int spawnX, spawnY;

	// Disable
	bool disableCharacter;

	public bool readyForInput {
		get {
			return !characterMovement.isMoving && !fallingBehavior.falling;
		}
	}

	void Start () {
		allowMovement = true;

		// Game Manager
		roomManager = Globals.roomManager;

		// Determine which player we are
		isLeftPlayer = (this == Globals.playerLeft);

		// Get other player
		if(isLeftPlayer){
			otherPlayer = Globals.playerRight;
		} else {
			otherPlayer = Globals.playerLeft;
		}

		// Get scripts
		characterMovement = GetComponent<CharacterMovementScript>();
		fallingBehavior = GetComponent<FallingBehaviorScript>();

		disableCharacter = false;

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
		// Temporary hack to reset camera
		// Might not be needed now? 
		roomManager.Reset();

		ResetPlayer();
		otherPlayer.ResetPlayer();
	}
	
	void Update () {

		// Disabled char
		if(disableCharacter){
			return;
		}

		// Check if we fell
		if(fallingBehavior.fell){
			ResetBothPlayers();
			return;
		}
	}

	// Make character essentially non-existant, used when one side is faded out
	public void DisableCharacter(){
		disableCharacter = true;
		shadow.SetActive(false);
	}

	public void EnableCharacter(){
		disableCharacter = false;
		shadow.SetActive(true);
	}


	// Will moving in the indicated direction move the player out of the room?
	public bool WillMoveOffScreen(Direction direction){

		if(disableCharacter)
			return true;

		Vector2 dest = new Vector2(tileX, tileY) + 2 * Utils.DirectionToVector(direction);
		return !roomManager.ContainsTile(dest);

	}

	public void GiveInputDirection(Direction direction){
		if(disableCharacter)
			return;

		if(fallingBehavior.falling || !allowMovement)
			return;

		// Check if we need to wait at the edge of the screen
		bool needToWait = WillMoveOffScreen(direction) && !otherPlayer.WillMoveOffScreen(direction);

		if(!needToWait){
			characterMovement.MoveInDirection(direction);
		}

	}

	public void EnableMovement(){
		allowMovement = true;
	}

	public void DisableMovement(){
		allowMovement = false;

	}

	public void DisableMovement(float t){
		allowMovement = false;
		CancelInvoke("EnableMovement");
		Invoke ("EnableMovement", t);
	}

}
