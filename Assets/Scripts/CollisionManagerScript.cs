using UnityEngine;
using System.Collections.Generic;

public class CollisionManagerScript : MonoBehaviour {

	[Tooltip("Layers which are checked by CollisionManager, all other layers will be ignored, " + 
	         "except in methods where a specific layer is passed in")]
	public LayerMask collidingLayers;

	/// <summary>
	/// Gets a list of Collider2D's on a tile.
	/// </summary>
	public Collider2D[] ObjectsOnTile(Vector2 tileCoords){
		Collider2D[] cols = Physics2D.OverlapPointAll(tileCoords, collidingLayers);
		return cols;
	}

	/// <summary>
	/// Gets a list of ColliderScripts's on a tile.
	/// </summary>
	public ColliderScript[] ColliderScriptsOnTile(Vector2 tileCoords){
		Collider2D[] cols = ObjectsOnTile(tileCoords);
		List<ColliderScript> scripts = new List<ColliderScript>();
		foreach(Collider2D c in cols){
			ColliderScript cs = c.GetComponent<ColliderScript>();
			if(cs != null){
				scripts.Add(cs);
			}
		}
		return scripts.ToArray();
	}

	/// <summary>
	/// Gets the ColliderScript of the object that's blocking a tile
	/// </summary>
	public ColliderScript TileBlocker(Vector2 tileCoords){
		foreach(ColliderScript cs in ColliderScriptsOnTile(tileCoords)){
			if(cs.blocking)
				return cs;
		}
		return null;
	}

	/// <summary>
	/// Gets the ColliderScript of the object that's a pit
	/// </summary>
	public ColliderScript Pit(Vector2 tileCoords){
		foreach(ColliderScript cs in ColliderScriptsOnTile(tileCoords)){
			if(cs.pit)
				return cs;
		}
		return null;
	}

	/// <summary>
	/// Is the tile blocking movement?
	/// </summary>
	public bool TileBlocking(Vector2 tileCoords){
		return TileBlocker(tileCoords) != null;
	}

	/// <summary>
	/// Is the tile a pit?
	/// </summary>
	public bool TileIsPit(Vector2 tileCoords){
		return Pit(tileCoords) != null;
	}



}
