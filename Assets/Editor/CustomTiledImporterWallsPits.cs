using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
public class CustomTiledImporterWallsPits : Tiled2Unity.ICustomTiledImporter {

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
	}
	
	public void CustomizePrefab(GameObject prefab){
		// Look for layers that contain "Collisions" or "Pits"
		// After some changes, now grabs from the first child of Map, which should be child object of the main prefab.
		foreach(Transform child in prefab.transform.GetChild(0)){

			if(child.name.Contains("Collisions")){

				// Look at each collider (wall) in that layer
				foreach(Transform child2 in child.transform){
					GameObject wall = child2.gameObject;

					// Give each wall a ColliderScript with blocking
					ColliderScript cs = wall.AddComponent<ColliderScript>();
					cs.blocking = true;

				}

			} else if(child.name.Contains("Pits")){

				// Look at each collider (pit) in that layer
				foreach(Transform child2 in child.transform){
					GameObject pit = child2.gameObject;
					
					// Give each pit a ColliderScript with blocking
					ColliderScript cs = pit.AddComponent<ColliderScript>();
					cs.pit = true;
					
				}
			}
		}
	}

}
