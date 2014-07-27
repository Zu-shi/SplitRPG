using UnityEngine;
using System.Collections;

public class CharacterMovementScript : _Mono {

	// Time it takes to move two spaces in frames (at Unity's fixed time step aka 50 fps)
	int moveTime = 12;

	int moveTimeLeft;
	float moveSpeed;

	bool _isMoving;
	public bool isMoving{
		get {
			return _isMoving;
		}
	}
	
	Vector2 moveVelocity;

	void Start () {

		moveTimeLeft = 0;

		// Calculate moveSpeed based on moveTime
		moveSpeed = 2f / (moveTime * Time.fixedDeltaTime);

		moveVelocity = new Vector2(0,0);
		_isMoving = false;

	}
	
	void FixedUpdate () {

		if(_isMoving){
			// Set character velocity
			rigidbody2D.velocity = moveVelocity;

			// Is move done yet?
			moveTimeLeft--;
			if(moveTimeLeft <= 0){
				StopMoving();
			}
		}

	}

	void StopMoving(){

		// Cancel move velocity
		rigidbody2D.velocity = new Vector2(0, 0);

		// Round location to nearest tile
		x = tileX;
		y = tileY;

		_isMoving = false;
		moveVelocity = new Vector2(0,0);

	}

	bool CanMoveInDirection(Direction direction){

		// Look for blocking tile
		ColliderScript blocker = Globals.CollisionManager.TileBlocker(tileVector + 2 * Utils.DirectionToVector(direction));

		// If we found one, try to push it
		if(blocker != null){
			if(!blocker.TryToPush(gameObject, direction)){
				// If we can't push it, we can't move
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Moves the character in the specified direction by 2 tiles
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void MoveInDirection(Direction direction){
		if(_isMoving || direction == Direction.NONE){
			return;
		}

		_isMoving = true;
		moveTimeLeft = moveTime;

		if(CanMoveInDirection(direction)){
			moveVelocity = Utils.DirectionToVector(direction) * moveSpeed;
		}
	}
}
