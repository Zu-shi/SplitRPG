using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterHeight : Tiled2Unity.ICustomTiledImporter{
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if (props.ContainsKey("height")) {
			HeightScript hs = GetHeightScript(gameObject);
			hs.height = int.Parse(props["height"]);
		}
		if (props.ContainsKey("drawingOrder")) {
			HeightScript hs = GetHeightScript(gameObject);
			string value = props["drawingOrder"].ToLower();
			for(DrawingOrder d = 0; d < DrawingOrder.NUM_DRAWING_ORDERS; d++){
				if(value.Equals(d.ToString().ToLower())){
					hs.drawingOrder = d;
					break;
				}
			}
		}
	}

	HeightScript GetHeightScript(GameObject o){
		HeightScript hs = o.GetComponent<HeightScript>();
		if(hs == null){
			hs = o.AddComponent<HeightScript>();
		}
		return hs;
	}

	public void CustomizePrefab(GameObject prefab){
		
	}
}