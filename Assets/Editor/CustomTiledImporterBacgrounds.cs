using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
public class CustomTiledImporterBackgrounds : Tiled2Unity.ICustomTiledImporter {

	private Object prefabToAdd;

	private string pathPrefix = "Assets/Prefabs/";

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props) {
		if(props.ContainsKey("loadPrefab")) {
			//Debug.Log("Found loadPrefab property: " + pathPrefix + props["loadPrefab"]);
			prefabToAdd = AssetDatabase.LoadAssetAtPath(pathPrefix + props["loadPrefab"] + ".prefab", typeof(GameObject));
		}
	}
	
	public void CustomizePrefab(GameObject prefab) {
		GameObject map = new GameObject("Map");
		int count = prefab.transform.childCount;

		for(int i = 0; i < count; i++) {
			prefab.transform.GetChild(0).parent = map.transform;
		}
		map.transform.localScale = new Vector3(0.015625f, 0.015625f, 0.015625f);
		prefab.transform.localScale = new Vector3(1,1,1);
		map.transform.parent = prefab.transform;
		if(prefabToAdd) {
			GameObject bg = (GameObject)GameObject.Instantiate(prefabToAdd);
			bg.transform.parent = prefab.transform;
		}
		else {
			Debug.Log("No loadPrefab property found on this map: " + prefab.name);
		}
	}
}