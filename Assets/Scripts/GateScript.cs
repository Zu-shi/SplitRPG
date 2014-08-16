using UnityEngine;
using System.Collections;

public class GateScript : _Mono {

	// Gate determines whether it's open or closed based on the state of switchToWatch
	public Toggler togglerToWatch; //{get; set;}
	public bool horizontal = true;
	public AudioClip openSound;
	public AudioClip closeSound;

	[Tooltip("Sprite shown when gate is open.")]
	public Sprite openSprite;

	[Tooltip("Sprite shown when gate is closed (Horizontal).")]
	public Sprite closedSpriteH;
	
	[Tooltip("Sprite shown when gate is closed (Vertical).")]
	public Sprite closedSpriteV;

	[Tooltip("Make the gate OPEN when its switch is off.")]
	public bool reverse = false;
	
	private Sprite closedSprite;

	private bool _isOpenLastFrame;
	public bool IsOpenLastFrame(){
		return _isOpenLastFrame;
	}

	public bool IsOpen() {
		bool isOpen;
		if (togglerToWatch.on != reverse) {
			isOpen = true;
		} else {
			isOpen = false;
		}
		return isOpen;
	}

	void Update () {
		//Debug.LogWarning ("toggler state " + togglerToWatch.on);
		if (horizontal) {
			closedSprite = closedSpriteH;
		} else {
			closedSprite = closedSpriteV;
		}
		
		bool open = togglerToWatch.on;
		if(reverse)
			open = !open;

		if ( IsOpen() != IsOpenLastFrame() ) {
			if(IsOpen()){
				Globals.soundManager.PlaySound(openSound);
			}else{
				Globals.soundManager.PlaySound(closeSound);
			}
		}

		if(open){
			spriteRenderer.sprite = openSprite;
		} else {
			spriteRenderer.sprite = closedSprite;
		}
		
		_isOpenLastFrame = IsOpen ();
	}
}
