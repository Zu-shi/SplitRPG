using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter(Order = 10)]
public class CustomTiledImporterMovementFlipTiles : Tiled2Unity.ICustomTiledImporter {

	/*
	 * To use movement flip tiles, the tiles must be specified in Tiled with either a 'flipXMovement' attribute
	 * or a 'flipYMovement' attribute. Additionally, the values of these attributes need to be set to either
	 * 'left' or 'right' depending on which side it is supposed to flip.
	 */
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
			} else {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = false;
			}
			if(props["flipXMovement"].Contains("right")) {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectRight = true;
			} else {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectRight = false;
			}
		}
		if(props.ContainsKey("flipYMovement")) {
			gameObject = MakePrefab(gameObject, verticalFlipper);
			if(props["flipYMovement"].Contains("left")) {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = true;
			} else {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectLeft = false;
			}
			if(props["flipYMovement"].Contains("right")) {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectRight = true;
			} else {
				gameObject.GetComponent<PlayerMovementFlipScript>().affectRight = false;
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
