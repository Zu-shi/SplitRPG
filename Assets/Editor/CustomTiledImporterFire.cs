using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter(Order = 6)]

public class CustomTiledImporterFire : Tiled2Unity.ICustomTiledImporter {

	public List<GameObject> fire1 = new List<GameObject>();
	public List<GameObject> fire2 = new List<GameObject>();
	public List<GameObject> fire3 = new List<GameObject>();

	private int counter = 0;

	private GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Fire.prefab", typeof(GameObject)) as GameObject;
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if(gameObject.transform.parent != null && gameObject.transform.parent.name == "Fire"){
			gameObject = MakePrefab(gameObject, prefab);
			if(props["level"] == "1")
				fire1.Add(gameObject);
			else if(props["level"] == "2")
				fire2.Add(gameObject);
			else if(props["level"] == "3")
				fire3.Add(gameObject);
		}

	}
	
	public void CustomizePrefab(GameObject prefab){
		if(fire1.Count != 0 || fire2.Count != 0 || fire3.Count != 0) {
			FireManagerScript tmp = prefab.AddComponent<FireManagerScript>();
			tmp.fire1 = fire1;
			tmp.fire2 = fire2;
			tmp.fire3 = fire3;
		}
	}

	private GameObject MakePrefab(GameObject o, GameObject prefab) {
		GameObject tmp = GameObject.Instantiate(prefab, o.transform.position, Quaternion.identity) as GameObject;
		tmp.name = o.name;
		o.name = o.name + LayerMask.LayerToName(o.layer) + counter;
		counter++;
		tmp.transform.parent = o.transform;
		tmp.transform.localScale *= 64;
		tmp.transform.position += 64 * new Vector3(1,-1,0);
		return tmp;
	}
}
