using UnityEngine;
using System.Collections;

public class MovementScript : _Mono {

	public bool canPush = false;
	public bool canFall = true;

	[Tooltip ("Object that will be scaled down when it falls.")]
	public Transform fallObject;

	[Tooltip ("Destroy the object after it falls?")]
	public bool destroyOnFall = true;
	
	[Tooltip ("Sound to play when the object falls.")]
	public AudioClip fallingSound;

	/// <summary>
	/// Object is unable to fall while inAir, e.g. if jumping or flying over a gap
	/// </summary>
	public bool inAir{get;set;}
	
	/// <summary>
	/// Object is currently falling
	/// </summary>
	public bool falling{get; set;}
	
	/// <summary>
	/// Object has fallen all the way down to invisible size.
	/// </summary>
	public bool fell{get;set;}

	// Time it takes to move two spaces in frames (at Unity's fixed time step aka 50 fps)
	const int moveTime = 11;
	
	// Time it takes to change direction in frames (at Unity's fixed time step aka 50 fps)
	const int changingDirectionTime = 5;

	// Time since last movement in frames 
	const int fastDirectionChangeThreshold = 12;

	Vector3 startScale;
	
	int moveTimeLeft;
	float moveSpeed;
	int fastDirectionChangeTimeLeft = 0;
	
	bool _isMoving;
	public bool isMoving{
		get {
			return _isMoving;
		}
	}
	
	public bool justMoved{
		get {
			return fastDirectionChangeTimeLeft > 0;
		}
	}

	int changingDirectionTimeLeft;

	bool _isChangingDirection;
	public bool isChangingDirection{
		get {
			return _isChangingDirection;
		}
	}

	Vector2 moveVelocity;
	public Direction moveDirection = Direction.NONE;
	
	void Start () {
		
		moveTimeLeft = 0;
		
		// Calculate moveSpeed based on moveTime
		moveSpeed = 2f / (moveTime * Time.fixedDeltaTime);
		
		moveVelocity = new Vector2(0,0);
		_isMoving = false;

		startScale = fallObject.localScale;
		ResetFalling();
	}

	/// <summary>
	/// Reset to not falling and regular size.
	/// </summary>
	public void ResetFalling (){
		falling = fell = false;
		inAir = false;
		fallObject.localScale = startScale;
		
	}

	protected void fallAnimation(){	
		Vector3 s = fallObject.localScale;
		s *= .9f;
		y -= 0.05f;
		fallObject.localScale = s;
		
		checkAndSetFell ();
	}

	protected void checkAndSetFell(){
		Vector3 s = fallObject.localScale;
		if (s.magnitude < .03f) {
			fell = true;
		}
	}

	protected virtual void StartFall(){	
		falling = true;
		Globals.soundManager.PlaySound(fallingSound);
		rigidbody2D.velocity = new Vector2 (0f, 0f);
	}

	void FixedUpdate () {

		if(canFall){
			if(fell && destroyOnFall){
				Destroy (gameObject);
				return;
			}
			
			if(!fell && !inAir){
				if (falling) {
					// Fall animation
					fallAnimation();
				} else {
					
					// Figure out if still on ground
					bool collidingWithPit = Globals.collisionManager.IsTilePit(xy, gameObject.layer);
					if (!inAir && collidingWithPit) {
						StartFall();
					}
				}
			}
		}

		if(isMoving){
			// Set character velocity
			rigidbody2D.velocity = moveVelocity;
			
			// Is move done yet?
			moveTimeLeft--;
			if(moveTimeLeft <= 0){
				StopMoving();
			}
		}

		if(isChangingDirection){
			
			// Is changing direction done yet?
			changingDirectionTimeLeft--;
			if(changingDirectionTimeLeft <= 0){
				StopChangingDirection();
			}

		}

		//If we are still allowed to change directions fast, decrease time required by 1.
		if(fastDirectionChangeTimeLeft > 0){
			fastDirectionChangeTimeLeft -= 1;
		}

	}

	public bool CanMoveInDirection(Direction direction){
		// Check if there is a fence blocking that direction
		if(Globals.collisionManager.IsFenceBlocking(this.tileVector, direction, this.gameObject.layer)) {
			//Debug.Log("Found fence, can't move.");
			return false;
		}
		
		// Look for blocking tile
		ColliderScript blocker = Globals.collisionManager.GetBlockingObject(this, direction);
	
		// If we found one, try to push it
		if(blocker != null){
			if(!canPush){
				return false;

			} else if(!blocker.TryToPush(gameObject, direction)){
				// If we can't push it, we can't move
				return false;
			}
		}
		
		return true;
	}
	
	/// <summary>
	/// Turns the character to face the specific direction.
	/// </summary>
	/// <param name="direction">Direction.</param>
	public bool ChangeDirection(Direction direction){
		if (_isChangingDirection || _isMoving || direction == Direction.NONE) {
				return false;
		}

		//Debug.Log ("Changing direction " + direction.ToString());
		//Debug.Log ("Current direction " + moveDirection.ToString());
		_isChangingDirection = true;
		moveDirection = direction;
		changingDirectionTimeLeft = changingDirectionTime;
		return false;
	}

	/// <summary>
	/// Stop the character to face the specific direction.
	/// </summary>
	protected void StopChangingDirection(){
		_isChangingDirection = false;
	}

	/// <summary>
	/// Moves the character in the specified direction by 2 tiles
	/// </summary>
	/// <param name="direction">Direction.</param>
	public bool MoveInDirection(Direction direction){
		if(_isMoving || _isChangingDirection || direction == Direction.NONE){
			return false;
		}
		
		_isMoving = true;
		moveTimeLeft = moveTime;
		moveDirection = direction;

		if(CanMoveInDirection(direction)){
			StartMoving(Utils.DirectionToVector(direction) * moveSpeed);
			return true;
		} else {
			return false;
		}
	}

	protected virtual void StartMoving(Vector2 velocity){
		collider2D.enabled = false;
		moveVelocity = velocity;
	}
	
	protected void StopMoving(){
		collider2D.enabled = true;
		// Cancel move velocity
		rigidbody2D.velocity = new Vector2(0, 0);
		
		// Round location to nearest tile
		x = tileX;
		y = tileY;
		
		_isMoving = false;
		moveVelocity = new Vector2(0,0);

		fastDirectionChangeTimeLeft = fastDirectionChangeThreshold;
	}
}
