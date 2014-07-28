using UnityEngine;
using System.Collections;

public class ButtonScript : _Mono {

	Switch _switch;

	[Tooltip("Sprite for when the button is pressed.")]
	public Sprite onSprite;

	[Tooltip("Sprite for when the button is not pressed.")]
	public Sprite offSprite;

	[Tooltip("Springy buttons spring back to the off state when the player walks off.")]
	public bool springy = false;

	[Tooltip("Gates that will watch the switch.")]
	public GateScript[] gates;
	
	void Start () {
		_switch = new Switch();

		// Tell each gate to watch our switch
		foreach(GateScript gate in gates){
			gate.switchToWatch = _switch;
		}
	}

	void Update () {

		// Is the player on us?
		bool playerIsOnUs = Globals.CollisionManager.PlayerIsOnTile(tileVector, gameObject.layer);

		// Switch the state of the switch if needed
		if(_switch.on && springy && !playerIsOnUs){
			_switch.TurnOff();
		} else if (_switch.off && playerIsOnUs){
			_switch.TurnOn();
		}

		// Update the sprite
		if(_switch.on){
			spriteRenderer.sprite = onSprite;
		} else {
			spriteRenderer.sprite = offSprite;
		}
	}

	public Switch GetSwitch(){
		return _switch;
	}
	
}
