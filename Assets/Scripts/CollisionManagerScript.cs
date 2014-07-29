using UnityEngine;
using System.Collections.Generic;

public class CollisionManagerScript : MonoBehaviour {

	[Tooltip("Layers checked when no side or layer is specified")]
	public LayerMask defaultLayers;

	[Tooltip("Layers checked for left side")]
	public LayerMask leftLayers;

	[Tooltip("Layers checked for left side")]
	public LayerMask rightLayers;
	
	/*
	 * Note about the "layerOfObject" parameter:
	 * 
	 * If the layer is "Left" or "Right" the CollisionManager will check
	 * the corresponding mask - leftLayers or rightLayers
	 * 
	 * If it's anything else, such as "Default" or -1, the Collision manager
	 * will check against the defaultLayers mask
	 * 
	 */

	/// <summary>
	/// Gets the ColliderScript of the object that's blocking a tile
	/// </summary>
	public ColliderScript GetBlockingObject(Vector2 tileCoords, int layerOfObject){
		foreach(ColliderScript cs in GetColliderScriptsOnTile(tileCoords, layerOfObject)){
			if(cs.blocking)
				return cs;
		}
		return null;
	}
	
	/// <summary>
	/// Gets the ColliderScript of the object that's a pit
	/// </summary>
	public ColliderScript GetPitObject(Vector2 tileCoords, int layerOfObject){
		foreach(ColliderScript cs in GetColliderScriptsOnTile(tileCoords, layerOfObject)){
			if(cs.pit)
				return cs;
		}
		return null;
	}
	
	/// <summary>
	/// Is the tile blocking movement?
	/// </summary>
	public bool IsTileBlocking(Vector2 tileCoords, int layerOfObject){
		return GetBlockingObject(tileCoords, layerOfObject) != null;
	}
	
	/// <summary>
	/// Is the tile a pit?
	/// </summary>
	public bool IsTilePit(Vector2 tileCoords, int layerOfObject){
		return GetPitObject(tileCoords, layerOfObject) != null;
	}
	
	/// <summary>
	/// Returns whether the player is on the specified tile
	/// </summary>
	public bool IsPlayerOnTile(Vector2 tileCoords, int layerOfObject){
		PlayerControllerScript player = Utils.PlayerOnLayer(layerOfObject);
		if(player.tileVector.Equals(tileCoords)){
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Gets a list of Collider2D's on a tile.
	/// </summary>
	private Collider2D[] GetObjectsOnTile(Vector2 tileCoords, int layerOfObject){
		LayerMask mask = defaultLayers;
		if(layerOfObject == LayerMask.NameToLayer("Left")){
			mask = leftLayers;
		} else if(layerOfObject == LayerMask.NameToLayer("Right")){
			mask = rightLayers;
		}
		Collider2D[] cols = Physics2D.OverlapPointAll(tileCoords, mask);
		return cols;
	}

	/// <summary>
	/// Gets a list of ColliderScripts's on a tile.
	/// </summary>
	private ColliderScript[] GetColliderScriptsOnTile(Vector2 tileCoords, int layerOfObject){
		Collider2D[] cols = GetObjectsOnTile(tileCoords, layerOfObject);
		List<ColliderScript> scripts = new List<ColliderScript>();
		foreach(Collider2D c in cols){
			ColliderScript cs = c.GetComponent<ColliderScript>();
			if(cs != null){
				scripts.Add(cs);
			}
		}
		return scripts.ToArray();
	}
	


}
