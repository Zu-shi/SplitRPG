using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter(Order = 4)]
public class CustomTiledImporterColorShift : Tiled2Unity.ICustomTiledImporter {
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		//Debug.Log("Handling " + gameObject.name);
		if(props.ContainsKey("colorShift")) {
			gameObject.transform.GetChild(0).gameObject.AddComponent<ColorShiftScript>();
		}
	}
	
	public void CustomizePrefab(GameObject prefab){}
}