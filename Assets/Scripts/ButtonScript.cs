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

	[Tooltip("Gates that will track the switch.")]
	public GateScript[] gates;
	
	void Start () {
		_switch = new Switch();

		foreach(GateScript gate in gates){
			gate.trackedSwitch = _switch;
		}
	}

	void Update () {
		bool playerIsOnUs = Utils.PlayerIsOnTile(tileX, tileY, gameObject.layer);

		if(_switch.on && springy && !playerIsOnUs){
			_switch.TurnOff();
		} else if (_switch.off && playerIsOnUs){
			_switch.TurnOn();
		}

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
