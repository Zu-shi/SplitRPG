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

	}
}