using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter(Order = short.MaxValue + 1)]
public class CustomTiledImporterObjectPersistence : Tiled2Unity.ICustomTiledImporter {

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		//Debug.Log("Handling " + gameObject.name);
		if(props.ContainsKey("Persistent")) {
			gameObject.tag = "Persistent";
		}
	}

	public void CustomizePrefab(GameObject prefab){
	}
}
