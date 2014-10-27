using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter(Order = short.MaxValue - 5)]
class CustomTiledImporterAddComp : Tiled2Unity.ICustomTiledImporter{

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if (gameObject != null && props.ContainsKey("AddComp")) {
			Debug.Log("Adding: " + props["AddComp"]);
			gameObject.AddComponent(props["AddComp"]);
		}
	}
	
	public void CustomizePrefab(GameObject prefab){}
}