using UnityEngine;
using System.Collections.Generic;

public class FireManagerScript : _Mono {

	public List<GameObject> fire1 = new List<GameObject>();
	public List<GameObject> fire2 = new List<GameObject>();
	public List<GameObject> fire3 = new List<GameObject>();
	
	public void PutOutFires() {
		//Fire estinguishing now managed in FireScript, which disables fire based on firelevel
		//This is in order to resolve lost references on map reload.
		Debug.Log(Globals.fireLevel);
		switch(Globals.fireLevel) {
		case 3:
			//foreach(GameObject fire in fire1)
			//	fire.SetActive(false);
			Globals.fireLevel = 2;
			Debug.Log("Firelevel changed: " + Globals.fireLevel);
			break;
		case 2:
			//foreach(GameObject fire in fire2)
			//	fire.SetActive(false);
			Globals.fireLevel = 1;
			Debug.Log("Firelevel changed: " + Globals.fireLevel);
			break;
		case 1:
			//foreach(GameObject fire in fire3)
			//	fire.SetActive(false);
			Globals.fireLevel = 0;
			Debug.Log("Firelevel changed: " + Globals.fireLevel);
			break;
		}
	}
}
