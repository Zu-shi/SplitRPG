using UnityEngine;
using System.Collections;

public class MovementScript : _Mono {

	public bool canPush = false;
	public bool canPushHeavy = false;
	public bool canFall = true;

	[Tooltip ("Object that will be scaled down when it falls.")]
	public Transform fallObject;

	[Tooltip ("Destroy the object after it falls?")]
	public bool destroyOnFall = true;
	
	[Tooltip ("Sound to play when the object falls.")]
	public AudioClip fallingSound;

	/// Object is unable to fall while inAir, e.g. if jumping or flying over a gap
	public bool inAir{get;set;}

	/// Object is currently falling
	public bool falling{get; set;}

	/// Object has fallen all the way down to invisible size.
	public bool fell{get;set;}

	// Parts of this pushblock, in terms of tiles offset from the original
	public Vector2[] body = {new Vector2(0f, 0f)};

	// Time it takes to move two spaces in frames (at Unity's fixed time step aka 50 fps)
	protected const int moveTime = 14;
	protected const int jumpTime = moveTime;
	
	// Time it takes to change direction in frames (at Unity's fixed time step aka 50 fps)
	protected const int changingDirectionTime = 5;

	protected Vector3 startScale;
	 
	protected int moveTimeLeft;
	protected int waitTimeLeft;
	protected float moveSpeed;
	protected float naturalCharacterOffset;

	// Time since last movement in frames 
	protected const int fastDirectionChangeThreshold = 12;
	protected int fastDirectionChangeTimeLeft = 0;
	protected const float maxJumpingHeight = 1f;
	protected Vector2 moveVelocity;
	
	protected bool _isMoving;
	public bool isMoving{
		get {
			return _isMoving;
		}
	}
	
	void Start () {

		//Set character jump height offset
		if (this.GetType () == typeof(CharacterMovementScript)) {
			_Mono sprite = gameObject.transform.FindChild("Sprite").GetComponent<_Mono>();
			_Mono parent = GetComponent<_Mono>();
			naturalCharacterOffset = sprite.y - parent.y;
		}

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
	public virtual void ResetFalling (){
		falling = fell = false;
		inAir = false;
		fallObject.localScale = startScale;
		fallObject.position = Vector3.zero;
		
	}

	protected virtual void fallAnimation(){	
		Vector3 s = fallObject.localScale;
		s *= .9f;
		fallObject.position -= new Vector3(0, 0.05f, 0);
		fallObject.localScale = s;
		
		checkAndSetFell ();
	}

	protected virtual void checkAndSetFell(){
		Vector3 s = fallObject.localScale;
		if (s.magnitude < .03f) {
			fell = true;
		}
	}

	protected virtual void StartFall(){	
		falling = true;
		Globals.soundManager.PlaySound(fallingSound);
		//rigidbody2D.velocity = new Vector2 (0f, 0f);
	}

	//Check if player will fall, and activate falling sequence if so.
	private void CheckAndStartFall(){
		bool collidingWithPit = true;
		foreach (Vector2 bodypart in body) {
			if( !Globals.collisionManager.IsTilePit(this.xy + bodypart, this.gameObject.layer) ) {;
				collidingWithPit = false;
			}
		}

		if (!inAir && collidingWithPit && !falling) {
			StartFall();
		}
	}

	protected virtual void FixedUpdate () {

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
					CheckAndStartFall();
					// Figure out if still on ground
					//Set character jump height offset
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

	}

	public bool CanMoveInDirectionWithPushSideEffect(Vector2 tileLocation, Direction direction){
		return CanMoveInDirection (tileLocation, direction, true);
	}
	
	public bool CanMoveInDirectionWithoutPushSideEffect(Vector2 tileLocation, Direction direction){
		return CanMoveInDirection (tileLocation, direction, false);
	}

	private bool CanMoveInDirection(Vector2 tileLocation, Direction direction, bool push){
		// Check if there is a fence blocking that direction
		foreach (Vector2 bodyPart in body){
			if( Globals.collisionManager.IsFenceBlocking(tileLocation + bodyPart, direction, this.gameObject.layer) ) {
				//Debug.Log("Found fence, can't move.");
				return false;
			}
		}
		
		// Look for blocking tile
		foreach (Vector2 bodyPart in body) {
			ColliderScript blocker = null;

			bool needToCheckForCollisions = true;
			foreach(Vector2 bodyPartTemp in body){
				if(bodyPartTemp == bodyPart + Utils.DirectionToVector(direction) * 2){
					needToCheckForCollisions = false; //This object does not need to check for collision.
				}
			}

			if(needToCheckForCollisions){
				blocker = Globals.collisionManager.GetBlockingObject (tileLocation + bodyPart, direction, this.gameObject.layer);
			}

			// This code must be changed if larger blocks can push other larger blocks, otherwise the code will work for large pushblocks.
			if(blocker != null){
				if(push){return blocker.TryToPush(this, direction);}
				else{return blocker.CanPush(this, direction);}
			}
		}
		
		return true;
	}

	/// <summary>
	/// Moves the character in the specified direction by 2 tiles
	/// </summary>
	/// <param name="direction">Direction.</param>
	public virtual bool MoveInDirection(Direction direction){
		if(_isMoving || direction == Direction.NONE){
			return false;
		}

		_isMoving = true;
		moveTimeLeft = moveTime;

		if(CanMoveInDirectionWithPushSideEffect(this.tileVector, direction)){
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
		inAir = false;
		CheckAndStartFall();

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
