using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
public class CustomTiledImporterObjectPersistence : Tiled2Unity.ICustomTiledImporter {

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if(props.ContainsKey("Persistent")) {
			gameObject.tag = "Persistent";
			Debug.Log("Persistent Object: " + gameObject.name);
		}
	}

	public void CustomizePrefab(GameObject prefab){
	}
}
