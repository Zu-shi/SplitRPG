using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterLeftRight : Tiled2Unity.ICustomTiledImporter{

	int physicsLayer;
	int roomsLayer;

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if(props.ContainsKey("side")){
			string key = props["side"];
			if(key.ToLower().Equals("left")){
				physicsLayer = LayerMask.NameToLayer("Left");
				roomsLayer = LayerMask.NameToLayer("RoomsLeft");
			} else if (key.ToLower().Equals("right")){
				physicsLayer = LayerMask.NameToLayer("Right");
				roomsLayer = LayerMask.NameToLayer("RoomsRight");
			}
		}
	}
	
	public void CustomizePrefab(GameObject prefab){
		//Debug.LogWarning ("Customizing");

		if(physicsLayer <= 0)
			return;

		SetPhysicsLayerRecursive(prefab, physicsLayer);
	}

	void SetPhysicsLayerRecursive(GameObject o, int layer){

		// If we're at the Rooms layer, start setting to roomsLayer instead
		if(o.name == "Rooms"){
			layer = roomsLayer;
		}

		// Set the layer, but only if it's currently on the "Default" layer
		if(o.layer == 0){
			o.layer = layer;
		}

		foreach(Transform child in o.transform){
			SetPhysicsLayerRecursive(child.gameObject, layer);
		}
	}
}