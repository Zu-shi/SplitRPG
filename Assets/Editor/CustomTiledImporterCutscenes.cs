using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter(Order = 50)]
public class CustomTiledImporterCutscenes : Tiled2Unity.ICustomTiledImporter{
	private string pathToPrefabs = "Assets/Prefabs/Cutscenes/";

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if (props.ContainsKey("Cutscene")) {
			GameObject pref = AssetDatabase.LoadAssetAtPath(pathToPrefabs + props["Cutscene"] + ".prefab", typeof(GameObject) ) as GameObject;

			if(pref == null) {
				Debug.LogError("Could not load cutscene prefab: " + props["Cutscene"]);
			}
			else {
				MakePrefab(gameObject, pref, props["Cutscene"]);
			}
		}
	}
	
	public void CustomizePrefab(GameObject prefab){}

	private GameObject MakePrefab(GameObject o, GameObject prefab, string name) {
		GameObject tmp = GameObject.Instantiate(prefab, o.transform.position, Quaternion.identity) as GameObject;
		tmp.name = name;
		tmp.transform.parent = o.transform;
		tmp.transform.localScale *= 64;
		tmp.transform.position += 64 * new Vector3(1,-1,0);
		return tmp;
	}
}
