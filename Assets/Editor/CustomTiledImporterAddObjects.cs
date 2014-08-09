using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEditor;
using System.Collections;

[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterAddObjects : Tiled2Unity.ICustomTiledImporter{

	private string pathPrefix = "Assets/Prefabs/";
	private Dictionary<string, GameObject> objects;


	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props) {
	}
	
	public void CustomizePrefab(GameObject prefab) {
		GameObject invisibleLayer;
		if (invisibleLayer = prefab.transform.FindChild ("Objects(Invisible)").gameObject) {
			
			GameObject tmp1 = new GameObject();
			tmp1.name = prefab.name;
			
			foreach(Transform child in invisibleLayer.transform)
			{
				string name = child.gameObject.name.ToLower();

				//GameObject item = (GameObject)GameObject.Instantiate(ImporterObjectPairs.mappings[name]);
				Debug.LogWarning("Trying to match object " + name );
				GameObject item = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/MappedObjects/" + name + ".prefab", typeof(GameObject)) as GameObject;
				if(item != null){
					//Add Vector3.one to offset center differences.
					item = (GameObject)GameObject.Instantiate(item, child.transform.position, child.transform.rotation);
					Debug.LogWarning("Adding object " + name );

					_Mono itemMono = item.GetComponent<_Mono>();
					itemMono.xs /= Utils.TILED_TO_UNITY_SCALE;
					itemMono.ys /= Utils.TILED_TO_UNITY_SCALE;
					itemMono.x += itemMono.xs/2;
					itemMono.y -= itemMono.ys/2;
					item.name = name;
					item.transform.parent = child;
				}
				//GameObject.Destroy(child);
			}
			
			PrefabUtility.CreatePrefab(pathPrefix + prefab.name + ".prefab", tmp1);
			GameObject.DestroyImmediate(tmp1);
		}
	}

}