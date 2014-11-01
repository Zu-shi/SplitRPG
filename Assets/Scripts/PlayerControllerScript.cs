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

	private bool _flipXMovement, _flipYMovement;
	private bool doubleTapToFallInPit = false;

	public bool hasFlipedXMovement{get{return _flipXMovement;}}

	/// <summary>
	/// Whether Player is ready to recieve input
	/// </summary>
	/// <value><c>true</c> if ready for input; otherwise, <c>false</c>.</value>
	public bool readyForInput {
		get {
			return !characterMovement.isMoving && !characterMovement.falling && allowCommands && !characterMovement.isChangingDirection;
		}
	}

	public void FlipXMovement() { _flipXMovement = !_flipXMovement; 
		if(horizontalDirection(characterMovement.moveDirection)) 
			characterMovement.moveDirection = FlipDirection(characterMovement.moveDirection);
	}
	public void FlipYMovement() { _flipYMovement = !_flipYMovement; characterMovement.moveDirection = FlipDirection(characterMovement.moveDirection);}

	public bool horizontalDirection(Direction d){
		return (d==Direction.LEFT || d==Direction.RIGHT);
	}

	void Start () {
		_flipXMovement = _flipYMovement = false;
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
		DisableMovement(.8f); // disable movement for a bit right when they spawn
		//flipXMovement = flipYMovement = false;
	}
	
	void ResetBothPlayers(){
		Globals.levelManager.LoadSerializedGame(true);


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

		if(InputManager.GetButtonDown(Button.RESET)){
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
	public bool WillMoveOffScreen(Direction readlDir){
		//Real direction used.

		if(disableCharacter)
			return true;

		Vector2 dest = new Vector2(tileX, tileY) + 2 * Utils.DirectionToVector(readlDir);
		Room room = roomManager.GetRoom(gameObject.layer);
		return characterMovement.CanMoveInDirectionWithoutPushSideEffect(characterMovement.tileVector, readlDir) && !room.ContainsTile(dest);

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

	private void Tap(Direction dir, bool keyDown) {
		if(doubleTapToFallInPit){
			if(characterMovement.WillFallInPit(dir) || otherPlayer.characterMovement.WillFallInPit(dir)) {
				if(keyDown) {
					characterMovement.MoveInDirection(dir);
				} else {
					characterMovement.PretendMoveInDirection(dir);
				}
			} else {
				characterMovement.MoveInDirection(dir);
			}
		}else{
			characterMovement.MoveInDirection(dir);
		}
	}

	private Direction FlipDirection(Direction dir) {
		if(dir == Direction.DOWN)
			return Direction.UP;
		if(dir == Direction.LEFT)
			return Direction.RIGHT;
		if(dir == Direction.RIGHT)
			return Direction.LEFT;
		if(dir == Direction.UP)
			return Direction.DOWN;
		return Direction.NONE;
	}

	/// <summary>
	/// Tells the player to move in a direction
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void GiveInputDirection(Direction direction, bool keyDown = false){
		if(disableCharacter)
			return;

		if(characterMovement.falling || !allowCommands)
			return;
		
		Direction realDir = direction;
		if(_flipXMovement && (direction == Direction.LEFT || direction == Direction.RIGHT))
		{realDir = FlipDirection(realDir);}
		if(_flipYMovement && (direction == Direction.UP || direction == Direction.DOWN))
		{realDir = FlipDirection(realDir);}

		Direction otherPlayerRealDir = direction;
		if(otherPlayer._flipXMovement && (direction == Direction.LEFT || direction == Direction.RIGHT))
		{otherPlayerRealDir = FlipDirection(otherPlayerRealDir);}
		if(otherPlayer._flipYMovement && (direction == Direction.UP || direction == Direction.DOWN))
		{otherPlayerRealDir = FlipDirection(otherPlayerRealDir);}


		// Check if we need to wait at the edge of the screen
		bool needToWait = WillMoveOffScreen(realDir) && !otherPlayer.WillMoveOffScreen(otherPlayerRealDir);

		if(!needToWait){
			if(WillMoveOffScreen(realDir))
				walkingOutOfRoom = true;

			//!characterMovement.justMoved
			if(characterMovement.moveDirection != realDir && !characterMovement.justMoved){
				characterMovement.ChangeDirection(realDir);
			}else{
				Tap(realDir, keyDown);
			}
		}else{
			if(WillMoveOffScreen(realDir)){
				if(characterMovement.moveDirection != realDir && !characterMovement.justMoved){
					characterMovement.ChangeDirection(realDir);
				}else{
					characterMovement.PretendMoveInDirection(realDir);
				}
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
