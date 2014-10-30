using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

//Sets a block on a default layer to show up on both sides.
//Note: switches are actually two seperate objects and thus does not need to be on the defaults layer.
[Tiled2Unity.CustomTiledImporter(Order = 12)]
public class CutstomTiledImporterAssignDefaultLayers : Tiled2Unity.ICustomTiledImporter {
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){}

	public void CustomizePrefab(GameObject prefab) {

		if (Utils.FindChildRecursive (prefab, "Pushblocks(Default)")) {
			SetToDefaultLayerInLayer (prefab, "Pushblocks(Default)");
		}

	}
		
	private void SetToDefaultLayerInLayer(GameObject prefab, string s){

		Transform defaultLayerTransform;
		if (defaultLayerTransform = Utils.FindChildRecursive(prefab, s)) {
			
			foreach(Transform t in defaultLayerTransform)
			{
				t.gameObject.layer = LayerMask.NameToLayer("Default");
				Debug.Log ("Set layer of " + t.gameObject.name + " to default");
			}
		}
	}
}
