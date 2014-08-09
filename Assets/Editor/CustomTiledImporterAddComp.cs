using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterAddComp : Tiled2Unity.ICustomTiledImporter{

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if (props.ContainsKey("AddComp")) {
			gameObject.AddComponent(props["AddComp"]);
		}
	}
	
	public void CustomizePrefab(GameObject prefab){
		// Look for layers that contain "Objects(Invisible)"
		foreach(Transform child in prefab.transform){
			
			if(child.name.Contains("Objects(Invisible)")){
				
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