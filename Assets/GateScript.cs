using UnityEngine;
using System.Collections;

public class GateScript : _Mono {

	public Switch trackedSwitch {get; set;}

	[Tooltip("Sprite shown when gate is open.")]
	public Sprite closedSprite;

	[Tooltip("Sprite shown when gate is closed.")]
	public Sprite openSprite;

	[Tooltip("Make the gate OPEN when its switch is off.")]
	public bool reverse = false;



	void Update () {
		bool open = trackedSwitch.on;
		if(reverse)
			open = !open;

		if(open){
			spriteRenderer.sprite = openSprite;
			collider2D.isTrigger = true;
		} else {
			spriteRenderer.sprite = closedSprite;
			collider2D.isTrigger = false;

		}
	}
}
