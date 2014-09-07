using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter(Order = short.MaxValue + 2)]
//Sets a block on a default layer to show up on both sides.
//Note: switches are actually two seperate objects and thus does not need to be on the defaults layer.
public class CustomTiledImporterConnectorAutofill : Tiled2Unity.ICustomTiledImporter {
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){}
	
	public void CustomizePrefab(GameObject prefab) {
		/*
		DefaultLayerConnectorScript dlcs;
		if ( (dlcs = Globals.gameManager.GetComponent<DefaultLayerConnectorScript> ()) != null ) {
			GameObject temp = null;
			if(dlcs.prefab2 == null){
				dlcs.prefab2 = prefab;
			}else if(dlcs.prefab2.name != prefab.name){
				temp = dlcs.prefab2;
			}else{
				temp = dlcs.prefab1;
			}

			dlcs.prefab2 = prefab;
			dlcs.prefab1 = temp;
			}
		}else{
			Debug.LogWarning("No default layer connector script found in gameManager, please attatch script.");
		}*/
	}
}
