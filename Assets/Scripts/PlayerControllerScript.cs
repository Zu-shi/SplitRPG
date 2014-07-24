using UnityEngine;
using System.Collections;

public class PlayerControllerScript : _Mono {

	bool allowMovement{get;set;}

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

	/// <summary>
	/// Whether Player is ready to recieve input
	/// </summary>
	/// <value><c>true</c> if ready for input; otherwise, <c>false</c>.</value>
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

	/// <summary>
	/// Resets the player to his spawn point
	/// </summary>
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

	/// <summary>
	/// Make character essentially non-existant, used when one side is faded out
	/// </summary>
	public void DisableCharacter(){
		disableCharacter = true;
		shadow.SetActive(false);
	}

	/// <summary>
	/// Make the character function again after disabling
	/// </summary>
	public void EnableCharacter(){
		disableCharacter = false;
		shadow.SetActive(true);
	}


	/// <summary>
	/// Will moving in the indicated direction move the player out of the room?
	/// </summary>
	public bool WillMoveOffScreen(Direction direction){

		if(disableCharacter)
			return true;

		Vector2 dest = new Vector2(tileX, tileY) + 2 * Utils.DirectionToVector(direction);
		return !roomManager.ContainsTile(dest);

	}

	/// <summary>
	/// Tells the player to move in a direction
	/// </summary>
	/// <param name="direction">Direction.</param>
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

	/// <summary>
	/// Enables the ability to accept movement commands.
	/// </summary>
	public void EnableMovement(){
		allowMovement = true;
	}

	/// <summary>
	/// Disables the ability to accept movement commands.
	/// </summary>
	public void DisableMovement(){
		allowMovement = false;

	}

	/// <summary>
	/// Disables the ability to accept movement commands for a duration.
	/// </summary>
	/// <param name="t">Duration in seconds.</param>
	public void DisableMovement(float t){
		allowMovement = false;
		CancelInvoke("EnableMovement");
		Invoke ("EnableMovement", t);
	}

}
