using UnityEngine;
using System.Collections.Generic;

public class CollisionManagerScript : MonoBehaviour {

	[Tooltip("Layers which are checked by CollisionManager, all other layers will be ignored, " + 
	         "except in methods where a specific layer is passed in")]
	public LayerMask collidingLayers;

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
	/// Is there a pit tile in the given direction in your movement?
	/// </summary>
	public bool TileBlockingInDirection(Vector2 tileCoords, Direction d){
		return Pit (tileCoords + Utils.DirectionToVector (d) * 2);
	}


	/// <summary>
	/// Is the tile blocking movement?
	/// </summary>
	public bool TileBlocking(Vector2 tileCoords){
		return TileBlocker(tileCoords) != null;
	}

	/// <summary>
	/// Is there a blocking tile in the given direction in your movement?
	/// </summary>
	public bool TilePitInDirection(Vector2 tileCoords, Direction d){
		return TileBlocking (tileCoords + Utils.DirectionToVector (d) * 2);
	}

	/// Is the tile a pit?
	/// </summary>
	public bool TileIsPit(Vector2 tileCoords){
		return Pit(tileCoords) != null;
	}

	/// <summary>
	/// Gets a list of Collider2Ds on a tile.
	/// </summary>
	private Collider2D[] ObjectsOnTile(Vector2 tileCoords){
		Collider2D[] cols = Physics2D.OverlapPointAll(tileCoords, collidingLayers);
		return cols;
	}
	
	/// <summary>
	/// Gets a list of ColliderScripts on a tile.
	/// </summary>
	private ColliderScript[] ColliderScriptsOnTile(Vector2 tileCoords){
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
}
