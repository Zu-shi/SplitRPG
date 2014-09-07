using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
public class CustomTiledImporterMovementFlipTiles : Tiled2Unity.ICustomTiledImporter {

	private GameObject horizontalFlipper, verticalFlipper;

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if(horizontalFlipper == null) {
			verticalFlipper = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tiles/VerticalMovementFlipper.prefab", typeof(GameObject)) as GameObject;
			horizontalFlipper = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tiles/HorizontalMovementFlipper.prefab", typeof(GameObject)) as GameObject;
		}
		if(props.ContainsKey("flipXMovement")) {
			gameObject = MakePrefab(gameObject, horizontalFlipper);
			if(props["flipXMovement"].Contains("left")) {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = true;
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = false;
			} else {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = false;
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = true;
			}
		}
		if(props.ContainsKey("flipYMovement")) {
			gameObject = MakePrefab(gameObject, verticalFlipper);
			if(props["flipXMovement"].Contains("left")) {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = true;
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = false;
			} else {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = false;
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = true;
			}
		}
	}

	public void CustomizePrefab(GameObject prefab){
	}

	private GameObject MakePrefab(GameObject o, GameObject prefab) {
		GameObject tmp = GameObject.Instantiate(prefab, o.transform.position, Quaternion.identity) as GameObject;
		tmp.name = o.name;
		tmp.transform.parent = o.transform.parent;
		tmp.transform.localScale *= 64;
		tmp.transform.position += 64 * new Vector3(1,-1,0);
		GameObject.DestroyImmediate(o);
		return tmp;
	}
}
