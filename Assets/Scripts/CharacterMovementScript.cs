using UnityEngine;
using System.Collections;

public class CharacterMovementScript : _Mono {

	public float moveSpeed;

	int moveTime;
	int moveTimeLeft;

	bool isMoving;

	float xv, yv;

	void Start () {

		// Calculate move time based on movespeed
		moveTime = (int)Mathf.Round(1 / (moveSpeed * Time.fixedDeltaTime));
		moveTimeLeft = 0;

		xv = yv = 0;
		isMoving = false;
	}
	
	void FixedUpdate () {

		if(isMoving){
			// Set character velocity
			rigidbody2D.velocity = new Vector2(xv, yv);

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

		isMoving = false;
		xv = yv = 0;

	}

	public void MoveInDirection(Direction direction){
		if(isMoving || direction == Direction.NONE){
			return;
		}

		isMoving = true;
		moveTimeLeft = moveTime;

		switch(direction){
		case Direction.LEFT:
			xv = -moveSpeed;
			break;
		case Direction.RIGHT:
			xv = moveSpeed;
			break;
		case Direction.UP:
			yv = moveSpeed;
			break;
		case Direction.DOWN:
			yv = -moveSpeed;
			break;
		}
	}
}
