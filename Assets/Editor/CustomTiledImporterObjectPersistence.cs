using UnityEngine;
using System.Collections.Generic;

public class CustomTiledImporterObjectPersistence : MonoBehaviour {

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if(props.ContainsKey("Persistent")) {
			gameObject.tag = "Persistent";
		}
	}

	public void CustomizePrefab(GameObject prefab){
	}
}
