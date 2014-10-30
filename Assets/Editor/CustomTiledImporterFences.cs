using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[Tiled2Unity.CustomTiledImporter(Order = 5)]
class CustomTiledImporterFences : Tiled2Unity.ICustomTiledImporter{
	
	private const bool DISABLE_IMPORT = false;
	private const string fencePostPath = "FencePost.prefab";
	private const string fenceHBarPath = "FenceHBar.prefab";
	private const string fenceVBarPath = "FenceVBar.prefab";
	private const string fenceVBarBottomPath = "FenceVBarBottom.prefab";
	private const string fenceVBarTopPath = "FenceVBarTop.prefab";
	private Object fencePost;
	private Object fenceHBar;
	private Object fenceVBar;
	private Object fenceVBarBottom;
	private Object fenceVBarTop;
	private const float yoffset = -0.22f;
	
	private Dictionary<string, string> prefabMap;
	private string mapName;

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props) {
		if(!DISABLE_IMPORT){
			if(gameObject.transform.parent == null){
				if(props.ContainsKey("map")){
					prefabMap = PrefabMapper.maps[props["map"]];
					//Added backslash here so that we can use load the default map without inserting a conditional.
					mapName = props["map"] + "/";
				}else{
					Debug.LogWarning("Map does not contain requisite 'map' property, default used.");
					prefabMap = PrefabMapper.maps["default"];
					Utils.assert(prefabMap != null);
					mapName = "";
				}
			}
		}
	}

	public void CustomizePrefab(GameObject prefab) {
		if(!DISABLE_IMPORT){
			 fencePost = AssetDatabase.LoadAssetAtPath(PrefabMapper.PrefabLocation + mapName + fencePostPath, typeof(GameObject));
			 fenceHBar = AssetDatabase.LoadAssetAtPath(PrefabMapper.PrefabLocation + mapName + fenceHBarPath, typeof(GameObject));
			 fenceVBarBottom = AssetDatabase.LoadAssetAtPath(PrefabMapper.PrefabLocation + mapName + fenceVBarBottomPath, typeof(GameObject));
			 fenceVBar = AssetDatabase.LoadAssetAtPath(PrefabMapper.PrefabLocation + mapName + fenceVBarPath, typeof(GameObject));
			 fenceVBarTop = AssetDatabase.LoadAssetAtPath(PrefabMapper.PrefabLocation + mapName + fenceVBarTopPath, typeof(GameObject));

			Transform tmp = Utils.FindChildRecursive(prefab, "Collisions");

			if(tmp) {
				GameObject graphics = new GameObject("Sprites");
				graphics.transform.parent = prefab.transform.FindChild("Map");

				for(int i = 0; i < tmp.childCount; i++) { 								// For each child of tmp
					EdgeCollider2D ec = tmp.GetChild(i).GetComponent<EdgeCollider2D>();
					if(ec) {
						//DebugEdgeCollider(ec);
						for(int j = 0; j < ec.pointCount; j++) { 						// For each point in the child's edge collider
							MakeFencePost(graphics, ec.points[j]/64 + (Vector2)ec.transform.position );
							if(j < ec.pointCount - 1) { 								// If we are not on the last point,
								MakeFenceBar(graphics, ec.points[j]/64 + (Vector2)ec.transform.position,
								             ec.points[j+1]/64 + (Vector2)ec.transform.position);		// connect this point to the next one.
							}
						}
					}
				}

				// Correct for camera offset.
				graphics.transform.position += new Vector3(-0.5f, 0.5f, 0);

				/*
				// Update the heights for edit mode.
				for(int i = 0; i < graphics.transform.childCount; i++) {
					for(int j = 0; j < graphics.transform.GetChild(i).childCount; j++) {
						graphics.transform.GetChild(i).GetChild(j).GetComponent<HeightScript>().editModeRefresh = true;
					}
				}
				*/
				
				// Update the heights for edit mode.
				for(int i = 0; i < graphics.transform.childCount; i++) {
					graphics.transform.GetChild(i).GetComponent<HeightScript>().editModeRefresh = true;
				}

			}
		}
	}

	private void MakeFencePost(GameObject graphics, Vector2 where) {
		GameObject tmp = (GameObject)GameObject.Instantiate(fencePost,
		                                                    where + new Vector2(0.5f, 0f + yoffset),
		                                                    Quaternion.identity);
		tmp.transform.parent = graphics.transform;
	}

	private void MakeFenceBar(GameObject graphics, Vector2 from, Vector2 to) {
		int lower, higher;
		if(from.x != to.x) { 														// This is a horizontal bar.
			lower = Mathf.Min ((int)from.x, (int)to.x);
			higher = Mathf.Max ((int)from.x, (int)to.x);
			for(int i = lower; i < higher; i++) {
				GameObject tmp = (GameObject)GameObject.Instantiate(fenceHBar,
				                                                    new Vector2(i + 1, from.y + yoffset),
			                                                    Quaternion.identity);
				tmp.transform.parent = graphics.transform;
			}

		}
		else {																		// This is a vertical bar.
			lower = Mathf.Min ((int)from.y, (int)to.y);
			higher = Mathf.Max ((int)from.y, (int)to.y);

			// Make the bottom bit of the bar.
			GameObject bottom = (GameObject)GameObject.Instantiate(fenceVBarBottom,
			                                                       new Vector2(from.x + 0.5f, lower + yoffset),
			                                                       Quaternion.identity);
			bottom.transform.parent = graphics.transform;

			// Make the middle bits of the bar.
			for(int i = lower; i < higher - 1; i++) {
				GameObject tmp = (GameObject)GameObject.Instantiate(fenceVBar,
				                                                    new Vector2(from.x + 0.5f, i + 2 + yoffset),
				                                                    Quaternion.identity);
				tmp.transform.parent = graphics.transform;
			}

			// Make the top bit of the bar.
			GameObject top = (GameObject)GameObject.Instantiate(fenceVBarTop,
			                                                    new Vector2(from.x + 0.5f, higher + yoffset),
			                                                       Quaternion.identity);
			top.transform.parent = graphics.transform;

		}

	}

	private void DebugEdgeCollider(EdgeCollider2D ec) {
		Debug.Log("Edge Collider : " + ec.transform.position);
		for(int i = 0; i < ec.pointCount; i++) {
			Debug.Log("\t" + ec.points[i]);
		}
	}

}
