using UnityEngine;
using System.Collections;

public class ButtonScript : _Mono {

	Toggler _toggler;
	public Toggler toggler{get{ return _toggler; }}

	[Tooltip("Sprite for when the button is pressed.")]
	public Sprite onSprite;

	[Tooltip("Sprite for when the button is not pressed.")]
	public Sprite offSprite;

	[Tooltip("Springy buttons spring back to the off state when the player walks off.")]
	public bool springy = false;

	[Tooltip("Gates that will watch the switch.")]
	public GateScript[] gates;
	
	void Start () {
		_toggler = new Toggler();

		// Tell each gate to watch our switch
		foreach(GateScript gate in gates){
			gate.togglerToWatch = _toggler;
		}
	}

	void Update () {

		// Is the player on us?
		bool playerIsOnButton = Globals.collisionManager.IsPlayerOnTile(tileVector, gameObject.layer);

		// Switch the state of the switch if needed
		if(_toggler.on && springy && !playerIsOnButton){
			_toggler.TurnOff();
		} else if (_toggler.off && playerIsOnButton){
			_toggler.TurnOn();
		}

		// Update the sprite
		if(_toggler.on){
			spriteRenderer.sprite = onSprite;
		} else {
			spriteRenderer.sprite = offSprite;
		}
	}


	
}
