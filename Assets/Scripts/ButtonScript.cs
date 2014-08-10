using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An in-game button that can be stepped on to toggle a target, such as a gate.
/// </summary>
/// <author>Mark Gardner</author>
public class ButtonScript : _Mono {

	private _Mono indicator;

	Toggler _toggler = new Toggler();
	public Toggler toggler{get{ return _toggler; }}

	[Tooltip("Sprite for when the button is pressed.")]
	public Sprite onSprite;

	[Tooltip("Sprite for when the button is not pressed.")]
	public Sprite offSprite;

	[Tooltip("Time for the switch to flip back to off state. " + 
	         "When set to 0 the button will switch off when the player walks off, "+
	         "when set to -1 the switch will stay on indefinitely")]
	public float timerLength = -1;
	float timeLeft;

	[Tooltip("Gates that will watch the switch.")]

	//Do NOT make this private! Prefab setting must be saved to a public setting.
	public List<GateScript> gates = new List<GateScript>();
	
	void Start () {
		timeLeft = timerLength;

		// Tell each gate to watch our switch
		foreach(GateScript gate in gates){
			gate.togglerToWatch = _toggler;
		}

		foreach (Transform child in transform)
		{
			if( child.name.Equals("TimerIndicator") ){
				indicator = child.GetComponentInChildren<_Mono> ();
				indicator.ys = 0;
				indicator.alpha = 0.45f;
				indicator.gameObject.layer = gameObject.layer;
			}
		}
	}

	void Update () {

		// Is the player on us?
		bool playerIsOnButton = Globals.collisionManager.IsPlayerOnTile(tileVector, gameObject.layer);

		// Toggle on if needed
		if (_toggler.off && playerIsOnButton){
			_toggler.TurnOn();
		}

		// Timer stuff
		// We only need to count down when we're toggled on
		// When timerLength is < 0 aka -1, the timer is disabled
		if(_toggler.on && timerLength >= 0){

			if(playerIsOnButton){
				timeLeft = timerLength; // reset timer

			} else {
				CountDownTimer();
			}
		}

		// Update the sprite
		if(_toggler.on){
			spriteRenderer.sprite = onSprite;
		} else {
			spriteRenderer.sprite = offSprite;
		}
	}

	void CountDownTimer(){
		if(timeLeft > 0){
			timeLeft -= Time.deltaTime; // count down
		}
		if(timeLeft <= 0){ // check if done
			_toggler.TurnOff();
		}
		
		if (timerLength != 0 && indicator != null) {
			indicator.ys = (timeLeft / timerLength);
		}
	}

	public void addGate(GateScript gs){
		gates.Add (gs);
		gs.togglerToWatch = _toggler;
		//Debug.LogWarning ("toggler passed");
		//Debug.LogWarning ("toggler value " + toggler.on);
	}
	
}
