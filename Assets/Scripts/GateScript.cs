using UnityEngine;
using System.Collections;

public class GateScript : _Mono {

	// Gate determines whether it's open or closed based on the state of switchToWatch
	public Switch switchToWatch {get; set;}

	[Tooltip("Sprite shown when gate is open.")]
	public Sprite closedSprite;

	[Tooltip("Sprite shown when gate is closed.")]
	public Sprite openSprite;

	[Tooltip("Make the gate OPEN when its switch is off.")]
	public bool reverse = false;

	ColliderScript colliderScript;

	void Start(){
		colliderScript = GetComponent<ColliderScript>();
	}

	void Update () {
		bool open = switchToWatch.on;
		if(reverse)
			open = !open;

		if(open){
			spriteRenderer.sprite = openSprite;
			// disable collision
			colliderScript.blocking = false;
		} else {
			spriteRenderer.sprite = closedSprite;
			// enable collision
			colliderScript.blocking = true;

		}
	}
}
