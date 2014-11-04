using UnityEngine;
using System.Collections;

public class ReadyToTeleportScript : _Mono {

	// Update is called once per frame
	void Update () {
		if(Globals.collisionManager.IsPlayerOnTile(tileVector, gameObject.layer)) {
			Globals.readyForTeleport = true;
		}else{
			Globals.readyForTeleport = false;
		}	
	}

}
