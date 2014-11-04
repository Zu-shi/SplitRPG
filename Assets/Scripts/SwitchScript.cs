using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An in-game button that can be stepped on to toggle a target, such as a gate.
/// </summary>
/// <author>Zuoming Shir</author>
public class SwitchScript : ColliderScript {

	private _Mono indicator;
	
	public Toggler _toggler = new Toggler();
	public SwitchScript twin = null;
	public bool toggledThisFrame {get; set;}
	//public Toggler toggler{get{ return _toggler; }}
	
	[Tooltip("Sprite for when the switch is activated.")]
	public Sprite onSprite;
	
	[Tooltip("Sprite for when the switch is not activated.")]
	public Sprite offSprite;
	
	[Tooltip("Gates that will watch the switch.")]
	//Do NOT make this private! Prefab setting must be saved to a public setting.
	public List<GateScript> gates = new List<GateScript>();
	
	//[Tooltip("DO NOT MODIFY. List of gates that are not connected to the other map.")]
	//public string remainingGates = "";

	void Start () {
		if (twin != null) {
			_toggler = twin._toggler;
			//Debug.Log ("Linked twins");
		}
		//if (remainingGates != "") {
		//	Debug.LogWarning("Gates unconnected to other map detected: " + remainingGates + ". Check DefaultSwitchesConnectorScript.");
		//}

		// Tell each gate to watch our switch
		foreach(GateScript gate in gates){
			gate.togglerToWatch = _toggler;
		}
	}

	//In order to maintain persistence, we need to pass references from old gates to new gates.
	public void InheritGatesFromSwitch(SwitchScript s){
		foreach(GateScript gate in s.gates){
			gate.togglerToWatch = _toggler;
		}
	}

	void Update () {
		toggledThisFrame = false;

		if (twin != null) {
			//_toggler = twin._toggler;

			//Debug.Log ("test equality: " + (_toggler == twin._toggler));
			//Debug.Log ("test side: " + _toggler.on + " vs. " + twin._toggler.on );
		}

		// Update the sprite
		if(_toggler.on){
			spriteRenderer.sprite = offSprite;
		} else {
			spriteRenderer.sprite = onSprite;
		}
	}
	
	public void addGate(GateScript gs){
		gates.Add (gs);
		//gs.togglerToWatch = _toggler;
	}

	public override void Activated(){
//		Debug.Log("Activated called");
		//_toggler.Toggle ();
		if(!toggledThisFrame){
			_toggler.Toggle();
		}

		toggledThisFrame = true;
		if(twin != null){
			twin.toggledThisFrame = true;
		}
	}
	
}
