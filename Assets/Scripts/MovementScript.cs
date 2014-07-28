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
	int moveTime = 12;

	Vector3 startScale;
	
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

		startScale = fallObject.localScale;
		Reset();
	}

	/// <summary>
	/// Reset to not falling and regular size.
	/// </summary>
	public void Reset (){
		falling = fell = false;
		inAir = false;
		fallObject.localScale = startScale;
		
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
					Vector3 s = fallObject.localScale;
					s *= .9f;
					fallObject.localScale = s;
					
					if (s.magnitude < .03f) {
						fell = true;
					}
					
				} else {
					
					// Figure out if still on ground
					bool collidingWithPit = Globals.CollisionManager.TileIsPit(xy);
					if (!inAir && collidingWithPit) {
						falling = true;
						Sound.PlaySound(fallingSound);
						rigidbody2D.velocity = new Vector2 (0f, 0f);
					}
				}
			}
			

		}


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

	void StartMoving(Vector2 velocity){
		collider2D.enabled = false;
		moveVelocity = velocity;

	}
	
	void StopMoving(){
		collider2D.enabled = true;

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
	/// Moves the character in the specified direction by 2 tiles
	/// </summary>
	/// <param name="direction">Direction.</param>
	public bool MoveInDirection(Direction direction){
		if(_isMoving || direction == Direction.NONE){
			return false;
		}
		
		_isMoving = true;
		moveTimeLeft = moveTime;
		
		if(CanMoveInDirection(direction)){
			StartMoving(Utils.DirectionToVector(direction) * moveSpeed);
			return true;
		} else {
			return false;
		}
	}
}
