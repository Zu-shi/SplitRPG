using UnityEngine;
using System.Collections;

public class FallingBehaviorScript : _Mono {

	[Tooltip ("Object that is treated as the center of the object and the object that will be scaled down when it falls.")]
	public Transform locAndScaleObject;

	[Tooltip("Radius of the circle to check against the ground plane")]
	public float radius = .1f;

	[Tooltip ("Destroy the object after it falls?")]
	public bool destroyOnFall;

	/// <summary>
	/// Object is unable to fall while inAir, e.g. if jumping or flying over a gap
	/// </summary>
	/// <value><c>true</c> if in air; otherwise, <c>false</c>.</value>
	public bool inAir{get;set;}

	/// <summary>
	/// Object is currently falling
	/// </summary>
	/// <value><c>true</c> if falling; otherwise, <c>false</c>.</value>
	public bool falling{get; set;}

	/// <summary>
	/// Object has fallen all the way down to invisible size.
	/// </summary>
	/// <value><c>true</c> if fell; otherwise, <c>false</c>.</value>
	public bool fell{get;set;}



	Vector3 startScale;

	LayerMask groundLayerMask;

	
	void Start () {
		startScale = locAndScaleObject.localScale;

		// Set the layer masks for what's considered ground
		// The object must be on the Left or Right layer for this to work right now
		if(gameObject.layer == LayerMask.NameToLayer("Left")){
			groundLayerMask = 1 << LayerMask.NameToLayer("GroundLeft");

		} else if (gameObject.layer == LayerMask.NameToLayer("Right")){
			groundLayerMask = 1 << LayerMask.NameToLayer("GroundRight");

		}

		Reset();
	}

	/// <summary>
	/// Reset to not falling and regular size.
	/// </summary>
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
