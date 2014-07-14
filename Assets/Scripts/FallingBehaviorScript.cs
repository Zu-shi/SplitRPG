using UnityEngine;
using System.Collections;

public class FallingBehaviorScript : _Mono {

	public Transform locAndScaleObject;

	public float radius = .1f;
	public bool destroyOnFall;

	public bool inAir{get;set;}
	public bool falling{get; set;}
	public bool fell{get;set;}

	Vector3 startScale;

	LayerMask groundLayerMask;

	
	void Start () {
		startScale = locAndScaleObject.localScale;

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
		
		// fall animation
		if (falling) {
			Vector3 s = locAndScaleObject.localScale;
			s *= .9f;
			locAndScaleObject.localScale = s;
			
			if (s.magnitude < .03f) {
				fell = true;
			}
		} else {
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
