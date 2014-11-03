using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter(Order = 10)]
public class CustomTiledImporterHints : Tiled2Unity.ICustomTiledImporter {
	
	private GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Hint.prefab", typeof(GameObject)) as GameObject;
	private GameObject prefab2 = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/LatentHint.prefab", typeof(GameObject)) as GameObject;
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if(gameObject.transform.parent != null && gameObject.transform.parent.name == "Hints"){
			gameObject = MakePrefab(gameObject, prefab);
		}	
		if(gameObject.transform.parent != null && gameObject.transform.parent.name == "Gradual Hints"){
			gameObject = MakePrefab(gameObject, prefab2);
			gameObject.AddComponent<HeightScript>();
			gameObject.GetComponent<HeightScript>().drawingOrder = DrawingOrder.NUM_DRAWING_ORDERS;
			gameObject.GetComponent<HeightScript>().height = 3;
		}	
	}
	
	public void CustomizePrefab(GameObject prefab){}

	private GameObject MakePrefab(GameObject o, GameObject prefab) {
		GameObject tmp = GameObject.Instantiate(prefab, o.transform.position, Quaternion.identity) as GameObject;
		tmp.name = o.name;
		tmp.transform.parent = o.transform.parent;
		tmp.transform.localScale *= 64;
		tmp.transform.position += 64 * new Vector3(1,-1,0);
		foreach(Component c in o.GetComponents<_Mono>())
			tmp.AddComponent(c.GetType());
		GameObject.DestroyImmediate(o);
		return tmp;
	}
}
