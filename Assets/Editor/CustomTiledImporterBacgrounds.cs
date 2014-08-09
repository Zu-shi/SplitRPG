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
		if(prefabToAdd) {
			GameObject tmp1 = new GameObject();
			tmp1.name = prefab.name;
			GameObject bg = (GameObject)GameObject.Instantiate(prefabToAdd);
			bg.name = "Background";
			bg.transform.parent = tmp1.transform;
			GameObject map = (GameObject)GameObject.Instantiate(prefab);
			map.name = "Map";
			map.transform.parent = tmp1.transform;
			PrefabUtility.CreatePrefab(pathPrefix + prefab.name + ".prefab", tmp1);
			GameObject.DestroyImmediate(tmp1);
		}
		else {
			Debug.Log("No loadPrefab property found on this map: " + prefab.name);
		}
	}
}