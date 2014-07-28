using UnityEngine;
using System.Collections;

public class ColliderScript : _Mono {

	[Tooltip("Blocks objects from being on top of them e.g. walls are blocking, buttons are not")]
	public bool blocking;

	[Tooltip("Player can interact with this by pressing the action button")]
	public bool activatable;

	[Tooltip("Players/objects can fall in")]
	public bool pit;

	/// <summary>
	/// Called when something enters the collider, e.g. block was pushed onto it, player stepped into it
	/// </summary>
	public virtual void ObjectEntered(GameObject obj){}

	/// <summary>
	/// Called when object is activated by the player
	/// </summary>
	public virtual void Activated(){}

	/// <summary>
	/// Tries to push the object in a direction
	/// </summary>
	/// <returns><c>true</c>, if the object was successfully pushed, <c>false</c> otherwise.</returns>
	public virtual bool TryToPush(GameObject pusher, Direction dir){
		return false;
	}

}
