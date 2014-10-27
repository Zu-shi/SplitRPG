using UnityEngine;
using System.Collections;

/// <summary>
/// Movement specific to a player character, attatched to player objects. 
/// This isn't really specific to the player yet, the Movement script stuff should be refactored here.
/// </summary>
/// <author>Mark Gardner</author>
public class CharacterMovementScript : MovementScript {

	public AudioClip walkingSound;
	public bool playWalkSound = true;
	public AudioClip wrongBeep;
	public Direction moveDirection = Direction.NONE;
	public bool canJump = false;
	public bool fallingInWater;
	public GameObject splashAnimation;
	protected int changingDirectionTimeLeft;
	protected bool _isChangingDirection;
	private const float wrongBeatWaitTime = 1.0f;
	
	public bool justMoved{
		get {
			return fastDirectionChangeTimeLeft > 0;
		}
	}
	
	public bool isChangingDirection{
		get {
			return _isChangingDirection;
			//return false;
		}
	}

	public void StopMovingNow() {
		moveDirection = Direction.NONE;
		fastDirectionChangeTimeLeft = 0;
		moveTimeLeft = 0;
		waitTimeLeft = 0;
	}

	protected override void FixedUpdate(){
		
		base.FixedUpdate();
		
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
		
		_Mono sprite = gameObject.transform.FindChild("Sprite").GetComponent<_Mono>();
		_Mono parent = GetComponent<_Mono>();

		if(!falling){
			if(inAir && isMoving){
				float fraction = (float)(moveTimeLeft) / (float)(moveTime) * 2;
				float offset = (- Mathf.Pow(fraction, 2) + 2 * fraction ) * maxJumpingHeight;
				sprite.y = parent.y + naturalCharacterOffset + offset;
			}else{sprite.y = parent.y + naturalCharacterOffset;}
		}

	}

	protected override void StartMoving( Vector2 velocity){
		base.StartMoving (velocity);
		if(playWalkSound)
			Globals.soundManager.PlaySound (walkingSound);
		//return yield (WaitForSeconds (1.0f))
	}

	protected void PlayWrongBeep(){
		Globals.soundManager.PlaySound (wrongBeep);
	}

	protected override void StartFall(){
		falling = true;
		totalFallTimer = 33;
		if(fallingInWater){
			gameObject.transform.FindChild("Sprite").GetComponent<HeightScript>().slightlyBelow = true;
			gameObject.transform.FindChild("Sprite").GetComponent<HeightScript>().drawingOrder = DrawingOrder.UNDER_GROUND;
			//base.StartFall ();
			Invoke ("CreateSplash", 0.1f);
			Invoke ("TurnInvisible", 0.2f);
		}else{
			Globals.soundManager.PlaySound(fallingSound);
		}
		Invoke("PlayWrongBeep", 0.6f);
	}

	protected void TurnInvisible(){
		gameObject.transform.FindChild("Sprite").GetComponent<_Mono>().alpha = 0f;
	}

	/// <summary>
	/// Reset to not falling and regular size.
	/// </summary>
	public override void ResetFalling (){
		falling = fell = false;
		inAir = false;
		fallObject.localScale = startScale;

		gameObject.transform.FindChild("Sprite").GetComponent<_Mono>().alpha = 1f;
		gameObject.transform.FindChild("Sprite").GetComponent<HeightScript>().slightlyBelow = false;
		gameObject.transform.FindChild("Sprite").GetComponent<HeightScript>().drawingOrder = DrawingOrder.OBJECTS;
	}

	protected void CreateSplash(){
		GameObject sp = Instantiate(splashAnimation, gameObject.transform.position, Quaternion.identity) as GameObject;
		_Mono m = sp.AddComponent<_Mono>();
		//m.x += Utils.DirectionToVector(moveDirection).x;
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
		return true;
	}

	public bool CanChangeDirection(Direction direction){
		if (_isChangingDirection || _isMoving || direction == Direction.NONE) {
			return false;
		}else return true;
	}
	
	/// <summary>
	/// Stop the character to face the specific direction.
	/// </summary>
	protected void StopChangingDirection(){
		_isChangingDirection = false;
	}

	protected override void fallAnimation(){

		if(!fallingInWater){
			Vector3 s = fallObject.localScale;
			s *= .9f;
			fallObject.gameObject.GetComponent<_Mono>().y -= 0.06f;
			fallObject.localScale = s;
			totalFallTimer -= 1;
		}else{
			Vector3 s = fallObject.localScale;
			//s -= 0.03f * Vector3.one;
			s *= 0.88f;
			fallObject.gameObject.GetComponent<_Mono>().y -= 0.2f;
			fallObject.localScale = s;
			totalFallTimer -= 1;
		}

		checkAndSetFell ();
	}

	/// <summary>
	/// This method pretends that the player is moving in order to sync the characters when one is against a wall while the other is at the exit.
	/// </summary>
	/// <param name="direction">Direction.</param>
	public bool PretendMoveInDirection(Direction direction){
		if(_isMoving || _isChangingDirection || direction == Direction.NONE){
			return false;
		}
		
		moveDirection = direction;
		_isMoving = true;
		moveTimeLeft = moveTime;
		StartMoving(Utils.DirectionToVector(direction) * 0.0f);
		return true;
	}

	public bool WillFallInPit(Direction dir) {
		if(Globals.collisionManager.IsTilePit(tileVector + Utils.DirectionToVector(dir), gameObject.layer)
		   && !Globals.collisionManager.IsFenceBlocking(tileVector, dir, this.gameObject.layer)) { // There is a pit in front of us
			if(canJump) {
				return Globals.collisionManager.IsTilePit(tileVector + Utils.DirectionToVector(dir) * 2, gameObject.layer);
			} else {
				return true;
			}
		} else {
			return false;
		}
	}

	/// <summary>
	/// Moves the character in the specified direction by 2 tiles, 4 if jumping.
	/// </summary>
	/// <param name="direction">Direction.</param>
	public override bool MoveInDirection(Direction direction){
		if(_isMoving || _isChangingDirection || direction == Direction.NONE){
			return false;
		}
		
		moveDirection = direction;
		_isMoving = true;
		moveTimeLeft = moveTime;
		Vector2 directionVector = Utils.DirectionToVector(direction);
		
		if(CanMoveInDirectionWithPushSideEffect(this.tileVector, direction)){
			if(!canJump){
				StartMoving(Utils.DirectionToVector(direction) * moveSpeed);
			}else{
				bool pitInFront = Globals.collisionManager.IsTilePit(xy + directionVector, gameObject.layer);
				//?
				bool safeToLand = CanMoveInDirectionWithoutPushSideEffect(xy + directionVector, direction);
				if (pitInFront && safeToLand && !JumpWillEnterNewRoom(direction)) {
					inAir = true;
					StartMoving(directionVector * moveSpeed * 2);
				}else{
					StartMoving(directionVector * moveSpeed);
				}
			}
			return true;
		} else {
			return false;
		}
	}

	private bool JumpWillEnterNewRoom(Direction dir) {
		//Debug.Log("Checking for new room on jump.");
		int roomLayer = Globals.roomManager.PlayerLayerToRoomLayer(gameObject.layer);
		Vector2 jumpCoords = xy + Utils.DirectionToVector(dir) * 4;
		//Debug.Log("\tCurrent coords: " + x + ", " + y);
		//Debug.Log("\tJump coords: " + jumpCoords.x + ", " + jumpCoords.y);
		BoxCollider2D currentRoom = Globals.roomManager.GetRoomAtPoint(xy, roomLayer);
		BoxCollider2D nextRoom = Globals.roomManager.GetRoomAtPoint(jumpCoords, roomLayer);

		if(currentRoom != nextRoom) { // The room we are in is not the same as the room we will enter.
			//Debug.Log("Rooms are different.");
			return true;
		} else {
			//Debug.Log("Rooms are the same.");
			return false;
		}
	}

}
