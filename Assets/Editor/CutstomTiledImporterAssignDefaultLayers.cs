using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

//Sets a block on a default layer to show up on both sides.
//Note: switches are actually two seperate objects and thus does not need to be on the defaults layer.
[Tiled2Unity.CustomTiledImporter(Order = 12)]
//NOTE: Must happen after layter
public class CutstomTiledImporterAssignDefaultLayers : Tiled2Unity.ICustomTiledImporter {
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){}

	public void CustomizePrefab(GameObject prefab) {

		if (Utils.FindChildRecursive (prefab, "Pushblocks(Default)")) {
			SetToDefaultLayerInLayer (prefab, "Pushblocks(Default)");
		}
		
		if (Utils.FindChildRecursive (prefab, "Fire(Default)")) {
			SetToDefaultLayerInLayer (prefab, "Fire(Default)");
		}
	}
		
	private void SetToDefaultLayerInLayer(GameObject prefab, string s){

		Transform defaultLayerTransform;
		if (defaultLayerTransform = Utils.FindChildRecursive(prefab, s)) {
			
			foreach(Transform t in defaultLayerTransform)
			{
				t.gameObject.layer = LayerMask.NameToLayer("Default");
				Debug.Log ("Set layer of " + t.gameObject.name + " to default");

				//For fire prefab
				Transform tc = t.GetChild(0);
				tc.gameObject.layer = LayerMask.NameToLayer("Default");
				Debug.Log ("Set layer of " + tc.gameObject.name + " to default");
			}
		}
	}
}
