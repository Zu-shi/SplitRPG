using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter(Order = short.MaxValue)]
//Sets a block on a default layer to show up on both sides.
public class CutstomTiledImporterPhysicsLayers : Tiled2Unity.ICustomTiledImporter {
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){}

	public void CustomizePrefab(GameObject prefab) {
		SetToDefaultLayerInLayer (prefab, "Pushblocks(Default)");
	}
		
	private void SetToDefaultLayerInLayer(GameObject prefab, string s){
		Transform defaultLayerTransform;
		if (defaultLayerTransform = Utils.FindChildRecursive(prefab,s)) {
			
			foreach(Transform t in defaultLayerTransform)
			{
				t.gameObject.layer = LayerMask.NameToLayer("Default");
			}
		}
	}
}
