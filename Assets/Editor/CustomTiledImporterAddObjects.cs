using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEditor;
using System.Collections;

[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterAddObjects : Tiled2Unity.ICustomTiledImporter{

	private string pathPrefix = "Assets/Prefabs/MappedObjects/";
	//Need to change name to "layerToHeightMap"

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props) {
	}
	
	public void CustomizePrefab(GameObject prefab) {
		GameObject invisibleLayer;

		if (invisibleLayer = prefab.transform.Find ("Objects(Invisible)").gameObject) {
	
			foreach(Transform obj in invisibleLayer.transform)
			{
				string name = obj.gameObject.name.ToLower();

				GameObject item = AssetDatabase.LoadAssetAtPath(pathPrefix + name + ".prefab", typeof(GameObject)) as GameObject;
				if(item != null){
					//Add Vector3.one to offset center differences.
					item = (GameObject)GameObject.Instantiate(item, obj.transform.position, obj.transform.rotation);

					_Mono itemMono = item.GetComponent<_Mono>();
					itemMono.xs /= Utils.TILED_TO_UNITY_SCALE;
					itemMono.ys /= Utils.TILED_TO_UNITY_SCALE;
					itemMono.x += itemMono.xs/2;
					itemMono.y -= itemMono.ys/2;
					item.name = name;
					item.transform.parent = obj;
					obj.name = obj.name + "Parent";
					item.layer = invisibleLayer.layer;

					HeightScript hs = Utils.GetHeightScript(item);
					hs.height = 2;
				}
				//GameObject.Destroy(child);
			}

		}
	}

}