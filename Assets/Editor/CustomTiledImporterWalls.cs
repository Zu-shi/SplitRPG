using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
public class CustomTiledImporterWalls : Tiled2Unity.ICustomTiledImporter {

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){

	}
	
	public void CustomizePrefab(GameObject prefab){
		// Look for layers that contain "Collisions"
		foreach(Transform child in prefab.transform){
			if(child.name.Contains("Collisions")){

				// Look at each collider (wall) in that layer
				foreach(Transform child2 in child.transform){
					GameObject wall = child2.gameObject;

					// Give each wall a ColliderScript with blocking
					ColliderScript cs = wall.AddComponent<ColliderScript>();
					cs.blocking = true;

				}
			}
		}
	}

}
