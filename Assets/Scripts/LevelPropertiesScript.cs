using UnityEngine;
using System.Collections;

public class LevelPropertiesScript : MonoBehaviour {

	public bool canJump = false;
	public bool canPushHeavy = false;
	public bool fallInWater = false;

	private CharacterMovementScript rightP, leftP;

	public void Start() {
		rightP = Globals.playerRight.GetComponent<CharacterMovementScript>();
		leftP = Globals.playerLeft.GetComponent<CharacterMovementScript>();
		if(rightP == null)
			Debug.LogError("Couldn't find right player's character movement script.");
		if(leftP == null)
			Debug.LogError("Couldn't find left player's character movement script.");
	}
	
	public void Sync() {
		leftP.canJump = canJump;
		rightP.canPushHeavy = canPushHeavy;
		rightP.fallingInWater = fallInWater;
		leftP.fallingInWater = fallInWater;
	}
}
