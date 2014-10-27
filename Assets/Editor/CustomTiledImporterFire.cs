using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
public class CustomTiledImporterFire : Tiled2Unity.ICustomTiledImporter {

	public List<GameObject> fire1 = new List<GameObject>();
	public List<GameObject> fire2 = new List<GameObject>();
	public List<GameObject> fire3 = new List<GameObject>();
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if(gameObject.transform.parent != null && gameObject.transform.parent.name == "Fire"){
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
}
