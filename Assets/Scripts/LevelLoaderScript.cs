using UnityEngine;
using System.Collections;

public class LevelLoaderScript : _Mono {

	public string leftLevel;
	public string rightLevel;

	public bool canJump = false;
	public bool canPush = false;

	public bool debugTileVectors = false;

	public void Update () {
		if(Globals.collisionManager.IsPlayerOnTile(tileVector, gameObject.layer)) {			// A player is on us
			if(canJump)
				Globals.playerLeft.GetComponent<CharacterMovementScript>().canJump = true;
			if(canPush)
				Globals.playerRight.GetComponent<CharacterMovementScript>().canPush = true;

			Globals.levelManager.LoadLevels(leftLevel, rightLevel);
		}
		if(debugTileVectors) {
			Debug.Log("Our tile vector: " + tileX + " , " + tileY);
			Debug.Log("Player's tile vector: " + Globals.playerRight.tileX + " , " + Globals.playerRight.tileY);
			debugTileVectors = false;
		}
	
	}
}
