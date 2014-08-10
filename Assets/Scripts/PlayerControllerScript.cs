using UnityEngine;
using System.Collections;

public class PlayerControllerScript : _Mono {

	bool allowCommands{get;set;}

	[Tooltip("Shadow object of the player")]
	public GameObject shadow;

	bool walkingOutOfRoom;
	
	// Room Manager
	RoomManagerScript roomManager;

	// Other player stuff
	bool isLeftPlayer;
	PlayerControllerScript otherPlayer;

	// Other scripts
	CharacterMovementScript characterMovement;

	// Disable
	bool disableCharacter;

	/// <summary>
	/// Whether Player is ready to recieve input
	/// </summary>
	/// <value><c>true</c> if ready for input; otherwise, <c>false</c>.</value>
	public bool readyForInput {
		get {
			return !characterMovement.isMoving && !characterMovement.falling && allowCommands;
		}
	}

	void Start () {
		allowCommands = true;

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

		disableCharacter = false;

	}

	/// <summary>
	/// Resets the player to his spawn point
	/// </summary>
	public void ResetPlayer(){
		characterMovement.ResetFalling();
	}
	
	void ResetBothPlayers(){
		Globals.levelManager.LoadLastCheckpoint();


		ResetPlayer();
		otherPlayer.ResetPlayer();
	}
	
	void Update () {

		// Disabled char
		if(disableCharacter){
			return;
		}

		// Walking out of room?
		if(walkingOutOfRoom && !characterMovement.isMoving){
			// This code doesn't work at all unless the characters are synced! Yay!
			shadow.transform.position = otherPlayer.transform.position;

			walkingOutOfRoom = false;
		}

		// Check if we fell
		if(characterMovement.fell){
			ResetBothPlayers();
			return;
		}

	}

//	void LateUpdate(){
//		// Update shadow
//		// needs work, but sort of functional
//		if(characterMovement.isMoving)
//			return;
//		if(Time.time < 1)
//			return;
//		Room room = roomManager.GetRoom(shadow.layer);
//		float sx = shadow.transform.position.x;
//		float sy = shadow.transform.position.y;
//		sx = Utils.Clamp(sx, room.roomLeft, room.roomRightTile);
//		sy = Utils.Clamp(sy, room.roomBotTile, room.roomTop);
//		shadow.transform.position = new Vector3(sx, sy, shadow.transform.position.z);
//	}

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
		Room room = roomManager.GetRoom(gameObject.layer);
		return characterMovement.CanMoveInDirection(direction) && !room.ContainsTile(dest);

	}
	
	/// <summary>
	/// Tells the player to act on object in front
	/// </summary>
	public void GiveInputAction(){
//		Debug.Log("disableCharacter");
		if(disableCharacter)
			return;
		
//		Debug.Log("characterMovement.falling || !allowCommands");
		if(characterMovement.falling || !allowCommands)
			return;

//		Debug.Log("GiveInputAction called");
		// Look for activating tile
		ColliderScript toActivate = Globals.collisionManager.GetActivatableObject(this, 
			GetComponent<CharacterMovementScript>().moveDirection);

		if (toActivate != null) {
			toActivate.Activated();
		}
	}

	/// <summary>
	/// Tells the player to move in a direction
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void GiveInputDirection(Direction direction){
		if(disableCharacter)
			return;

		if(characterMovement.falling || !allowCommands)
			return;

		// Check if we need to wait at the edge of the screen
		bool needToWait = WillMoveOffScreen(direction) && !otherPlayer.WillMoveOffScreen(direction);


		if(!needToWait){
			if(WillMoveOffScreen(direction))
				walkingOutOfRoom = true;

			if(characterMovement.moveDirection != direction && !characterMovement.justMoved){
				characterMovement.ChangeDirection(direction);
			}else{
				characterMovement.MoveInDirection(direction);
			}

		}

	}

	/// <summary>
	/// Enables the ability to accept movement commands.
	/// </summary>
	public void EnableMovement(){
		allowCommands = true;
	}

	/// <summary>
	/// Disables the ability to accept movement commands.
	/// </summary>
	public void DisableMovement(){
		allowCommands = false;

	}

	/// <summary>
	/// Disables the ability to accept movement commands for a duration.
	/// </summary>
	/// <param name="t">Duration in seconds.</param>
	public void DisableMovement(float t){
		allowCommands = false;
		CancelInvoke("EnableMovement");
		Invoke ("EnableMovement", t);
	}

}
