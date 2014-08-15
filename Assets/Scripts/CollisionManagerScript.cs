using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tests if a tile is a certain kind of collider, i.e. a pit or wall.
/// </summary>
/// <author>Mark Gardner</author>
public class CollisionManagerScript : MonoBehaviour {

	[Tooltip("Layers checked when no side or layer is specified")]
	public LayerMask defaultLayers;

	[Tooltip("Layers checked for left side")]
	public LayerMask leftLayers;

	[Tooltip("Layers checked for right side")]
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
	/// Is the tile blocking movement?
	/// </summary>
	public bool IsTileActivatable(Vector2 tileCoords, int layerOfObject){
		return GetActivatableObject(tileCoords, layerOfObject);
	}
	
	/// <summary>
	/// Is the tile in front of me activatable? Overload that takes coordinates, direction, and layer of object.
	/// </summary>
	public bool IsTileActivatable(Vector2 tileCoords, Direction direction, int layerOfObject){
		return IsTileActivatable(tileCoords + 2 * Utils.DirectionToVector(direction), layerOfObject);
	}
	
	/// <summary>
	/// Is the tile in front of me activatable? Overload that takes a mono and a direction.
	/// </summary>
	public bool IsTileActivatable(_Mono mono, Direction direction){
		return IsTileActivatable(mono.tileVector + 2 * Utils.DirectionToVector(direction), mono.gameObject.layer);
	}

	/// <summary>
	/// Gets the ColliderScript of the object that's activatable
	/// </summary>
	public ColliderScript GetActivatableObject(Vector2 tileCoords, int layerOfObject){
		foreach(ColliderScript cs in GetColliderScriptsOnTile(tileCoords, layerOfObject)){
			if(cs.activatable) return cs;
		}
		return null;
	}

	/// <summary>
	/// Gets the ColliderScript of the object that's activatable. Overload that takes coordinates, direction, and layer of object.
	/// </summary>
	public ColliderScript GetActivatableObject(Vector2 tileCoords, Direction direction, int layerOfObject){
		return GetActivatableObject(tileCoords + 2 * Utils.DirectionToVector(direction), layerOfObject);
	}

	/// <summary>
	/// Gets the ColliderScript of the object that's activatable. Overload that takes a mono and a direction.
	/// </summary>
	public ColliderScript GetActivatableObject(_Mono mono, Direction direction){
		return GetActivatableObject(mono.tileVector + 2 * Utils.DirectionToVector(direction), mono.gameObject.layer);
	}

	/// <summary>
	/// Gets the ColliderScript of the object that's blocking a tile
	/// </summary>
	public ColliderScript GetBlockingObject(Vector2 tileCoords, int layerOfObject){
		foreach(ColliderScript cs in GetColliderScriptsOnTile(tileCoords, layerOfObject)){
			if(cs.blocking) return cs;
		}
		return null;
	}

	/// <summary>
	/// Gets the ColliderScript of the object that's blocking a tile. Overload that takes a coordinate, a direction, and a layer
	/// </summary>
	public ColliderScript GetBlockingObject(Vector2 tileCoords, Direction direction, int layerOfObject){
		return GetBlockingObject(tileCoords + 2 * Utils.DirectionToVector(direction), layerOfObject);
	}

	/// <summary>
	/// Gets the ColliderScript of the object that's blocking a tile. Overload that takes an mono and a direction.
	/// </summary>
	public ColliderScript GetBlockingObject(_Mono mono, Direction direction){
		return GetBlockingObject(mono.tileVector + 2 * Utils.DirectionToVector(direction), mono.gameObject.layer);
	}
	
	/// <summary>
	/// Gets the ColliderScript of the object that's a pit
	/// </summary>
	public ColliderScript GetPitObject(Vector2 tileCoords, int layerOfObject){
		foreach(ColliderScript cs in GetColliderScriptsOnTile(tileCoords, layerOfObject)){
			if(cs.pit) return cs;
		}
		return null;
	}

	/// <summary>
	/// Gets the ColliderScript of the object that's a pit. Overload that takes a coordinate, a direction, and a layer
	/// </summary>
	public ColliderScript GetPitObject(Vector2 tileCoords, Direction direction, int layerOfObject){
		return GetPitObject(tileCoords + 2 * Utils.DirectionToVector(direction), layerOfObject);
	}
	
	/// <summary>
	/// GGets the ColliderScript of the object that's a pit. Overload that takes an mono and a direction.
	/// </summary>
	public ColliderScript GetPitObject(_Mono mono, Direction direction){
		return GetPitObject(mono.tileVector + 2 * Utils.DirectionToVector(direction), mono.gameObject.layer);
	}

	/// <summary>
	/// Is the tile blocking movement?
	/// </summary>
	public bool IsTileBlocking(Vector2 tileCoords, int layerOfObject){
		return GetBlockingObject(tileCoords, layerOfObject);
	}

	/// <summary>
	/// Is the tile blocking movement? Overload that takes coordinates, direction, and layer of object.
	/// </summary>
	public bool IsTileBlocking(Vector2 tileCoords, Direction direction, int layerOfObject){
		return IsTileBlocking(tileCoords + 2 * Utils.DirectionToVector(direction), layerOfObject);
	}

	/// <summary>
	/// Is the tile blocking movement? Overload that takes a mono and a direction.
	/// </summary>
	public bool IsTileBlocking(_Mono mono, Direction direction){
		return IsTileBlocking(mono.tileVector + 2 * Utils.DirectionToVector(direction), mono.gameObject.layer);
	}

	/// <summary>
	/// Is the tile a pit?
	/// </summary>
	public bool IsTilePit(Vector2 tileCoords, int layerOfObject){
		return GetPitObject(tileCoords, layerOfObject);
	}

	/// <summary>
	/// Is the tile a pit? Overload that takes coordinates, direction, and layer of object.
	/// </summary>
	public bool IsTilePit(Vector2 tileCoords, Direction direction, int layerOfObject){
		return IsTilePit(tileCoords + 2 * Utils.DirectionToVector(direction), layerOfObject);
	}
	
	/// <summary>
	/// Is the tile a pit? Overload that takes a mono and a direction.
	/// </summary>
	public bool IsTilePit(_Mono mono, Direction direction){
		return IsTilePit(mono.tileVector + 2 * Utils.DirectionToVector(direction), mono.gameObject.layer);
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
	/// Returns whether a player is inside a collider
	/// </summary>
	public bool IsPlayerCollidingWith(Collider2D aCollider, int layerOfObject){
		PlayerControllerScript player = Utils.PlayerOnLayer(layerOfObject);
		return aCollider.OverlapPoint(player.xy);
	}

	public bool IsFenceBlocking(Vector2 tileCoords, Direction direction, int layerOfObject) {
		LayerMask mask = defaultLayers;
		if(layerOfObject == LayerMask.NameToLayer("Left")){
			mask = leftLayers;
		} else if(layerOfObject == LayerMask.NameToLayer("Right")){
			mask = rightLayers;
		}
		RaycastHit2D[] hits = Physics2D.RaycastAll(tileCoords, Utils.DirectionToVector(direction), 2.0f, mask);
		foreach(RaycastHit2D hit in hits) {
			if(hit.collider.transform.parent != null && hit.collider.transform.parent.name == "Collisions") {
				return true;
			}
			GateScript gs = hit.collider.gameObject.GetComponent<GateScript>();
			if(gs != null) { // It's a gate
				return !gs.IsOpen();
			}
		}
		return false;
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
