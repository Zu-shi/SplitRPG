using UnityEngine;
using System.Collections;

public class FallingBehaviorScript : _Mono {

	// Object that is treated as the center of the object
	// and the object that will be scaled down when it falls
	public Transform locAndScaleObject;

	// Radius of the circle to check against the ground plane
	public float radius = .1f;

	// Destroy the object after it falls?
	public bool destroyOnFall;

	// Can set this to make the object unable to fall
	// For example if a character is jumping, set its inAir to true
	public bool inAir{get;set;}

	// Object is currently falling down
	public bool falling{get; set;}

	// Object has fallen all the way down to invisible size
	public bool fell{get;set;}



	Vector3 startScale;

	LayerMask groundLayerMask;

	
	void Start () {
		startScale = locAndScaleObject.localScale;

		// Set the layer masks for what's considered ground
		// The object must be on the Left or Right layer for this to work right now
		if(gameObject.layer == LayerMask.NameToLayer("Left")){
			groundLayerMask = 1 << LayerMask.NameToLayer("LeftGround");

		} else if (gameObject.layer == LayerMask.NameToLayer("Right")){
			groundLayerMask = 1 << LayerMask.NameToLayer("RightGround");

		}

		Reset();
	}
	
	public void Reset (){
		falling = fell = false;
		inAir = false;
		locAndScaleObject.localScale = startScale;

	}
	
	void Update () {
		if(fell && destroyOnFall){
			Destroy (gameObject);
			return;
		}
		
		if(fell || inAir){
			return;
		}
		
		if (falling) {

			// Fall animation
			Vector3 s = locAndScaleObject.localScale;
			s *= .9f;
			locAndScaleObject.localScale = s;
			
			if (s.magnitude < .03f) {
				fell = true;
			}

		} else {

			// Figure out if still on ground
			bool onGround = false;
			if (inAir || Physics2D.OverlapCircle (locAndScaleObject.position, radius, groundLayerMask)) {
				onGround = true;
			}
			
			if (!onGround) {
				falling = true;
				rigidbody2D.velocity = new Vector2 (0f, 0f);
			}
		}
		
	}
}
