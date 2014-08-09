using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;
using System.Collections;

[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterAddObjects : Tiled2Unity.ICustomTiledImporter{

	private Dictionary<string, GameObject> objects;

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		Transform parent = gameObject.transform.parent;
		if(parent == null)
			return;

	}
	
	public void CustomizePrefab(GameObject prefab){
		if( prefab.name.Contains("Objects(Invisble)") ) {
			if(prefab.name.Contains("pushblock")){
				//objects.Add(gameObject, ImporterObjectPairs.mappings[gameObject.transform.name]);
				GameObject model = ImporterObjectPairs.mappings[prefab.transform.name];
				GameObject obj = Utils.Create(model, 0f, 0f);
				obj.transform.parent = obj.transform;
			}
		}
	}
}