using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEditor;
using System.Collections;

[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterAddObjects : Tiled2Unity.ICustomTiledImporter{

	private string pathPrefix = "Assets/Prefabs/MappedObjects/";


	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props) {
	}
	
	public void CustomizePrefab(GameObject prefab) {
		GameObject invisibleLayer;

		if (invisibleLayer = prefab.transform.Find ("Objects(Invisible)").gameObject) {
	
			foreach(Transform child in invisibleLayer.transform)
			{
				string name = child.gameObject.name.ToLower();

				GameObject item = AssetDatabase.LoadAssetAtPath(pathPrefix + name + ".prefab", typeof(GameObject)) as GameObject;
				if(item != null){
					//Add Vector3.one to offset center differences.
					item = (GameObject)GameObject.Instantiate(item, child.transform.position, child.transform.rotation);

					_Mono itemMono = item.GetComponent<_Mono>();
					itemMono.xs /= Utils.TILED_TO_UNITY_SCALE;
					itemMono.ys /= Utils.TILED_TO_UNITY_SCALE;
					itemMono.x += itemMono.xs/2;
					itemMono.y -= itemMono.ys/2;
					item.name = name;
					item.transform.parent = child;
					item.layer = invisibleLayer.layer;
				}
				//GameObject.Destroy(child);
			}

		}
	}

}