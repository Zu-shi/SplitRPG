﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An in-game button that can be stepped on to toggle a target, such as a gate.
/// </summary>
/// <author>Zuoming Shir</author>
public class SwitchScript : ColliderScript {
	
	private _Mono indicator;
	
	Toggler _toggler = new Toggler();
	public Toggler toggler{get{ return _toggler; }}
	
	[Tooltip("Sprite for when the switch is activated.")]
	public Sprite onSprite;
	
	[Tooltip("Sprite for when the switch is not activated.")]
	public Sprite offSprite;
	
	[Tooltip("Gates that will watch the switch.")]
	//Do NOT make this private! Prefab setting must be saved to a public setting.
	public List<GateScript> gates = new List<GateScript>();
	
	void Start () {
		// Tell each gate to watch our switch
		foreach(GateScript gate in gates){
			gate.togglerToWatch = _toggler;
		}
	}
	
	void Update () {
		// Update the sprite
		if(_toggler.on){
			spriteRenderer.sprite = onSprite;
		} else {
			spriteRenderer.sprite = offSprite;
		}
	}
	
	public void addGate(GateScript gs){
		gates.Add (gs);
		//gs.togglerToWatch = _toggler;
	}

	public override void Activated(){
//		Debug.Log("Activated called");
		//_toggler.Toggle ();
		_toggler.Toggle();
	}
	
}