using UnityEngine;
using System.Collections;

public class CharacterMovementScript : _Mono {

	// Time it takes to move on space in frames (at Unity's fixed time step aka 50 fps)
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
		moveSpeed = 1f / (moveTime * Time.fixedDeltaTime);

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
		// Might do some checking in the future to predict if we're going to run into stuff
		return true;
	}

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
