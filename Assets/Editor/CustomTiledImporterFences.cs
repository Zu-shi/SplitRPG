using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[Tiled2Unity.CustomTiledImporter]
class CustomTiledImporterFences : Tiled2Unity.ICustomTiledImporter{

	private const string fencePostPath = "Assets/Prefabs/Tiles/FencePost.prefab";
	private const string fenceHBarPath = "Assets/Prefabs/Tiles/FenceHBar.prefab";
	private const string fenceVBarPath = "Assets/Prefabs/Tiles/FenceVBar.prefab";
	private const string fenceVBarBottomPath = "Assets/Prefabs/Tiles/FenceVBarBottom.prefab";
	private const string fenceVBarTopPath = "Assets/Prefabs/Tiles/FenceVBarTop.prefab";


	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props) {}

	public void CustomizePrefab(GameObject prefab) {
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

			// Update the heights for edit mode.
			for(int i = 0; i < graphics.transform.childCount; i++) {
				for(int j = 0; j < graphics.transform.GetChild(i).childCount; j++) {
					graphics.transform.GetChild(i).GetChild(j).GetComponent<HeightScript>().editModeRefresh = true;
				}
			}

		}
	}

	private void MakeFencePost(GameObject graphics, Vector2 where) {
		GameObject tmp = (GameObject)GameObject.Instantiate(AssetDatabase.LoadAssetAtPath(fencePostPath, typeof(GameObject)),
		                                                    where,
		                                                    Quaternion.identity);
		tmp.transform.parent = graphics.transform;
	}

	private void MakeFenceBar(GameObject graphics, Vector2 from, Vector2 to) {
		int lower, higher;
		if(from.x != to.x) { 														// This is a horizontal bar.
			lower = Mathf.Min ((int)from.x, (int)to.x);
			higher = Mathf.Max ((int)from.x, (int)to.x);
			for(int i = lower; i < higher; i++) {
			GameObject tmp = (GameObject)GameObject.Instantiate(AssetDatabase.LoadAssetAtPath(fenceHBarPath, typeof(GameObject)),
			                                                    new Vector2(i + 1, from.y),
			                                                    Quaternion.identity);
				tmp.transform.parent = graphics.transform;
			}

		}
		else {																		// This is a vertical bar.
			lower = Mathf.Min ((int)from.y, (int)to.y);
			higher = Mathf.Max ((int)from.y, (int)to.y);

			// Make the bottom bit of the bar.
			GameObject bottom = (GameObject)GameObject.Instantiate(AssetDatabase.LoadAssetAtPath(fenceVBarBottomPath, typeof(GameObject)),
			                                                       new Vector2(from.x, lower),
			                                                       Quaternion.identity);
			bottom.transform.parent = graphics.transform;

			// Make the middle bits of the bar.
			for(int i = lower; i < higher - 1; i++) {
				GameObject tmp = (GameObject)GameObject.Instantiate(AssetDatabase.LoadAssetAtPath(fenceVBarPath, typeof(GameObject)),
				                                                    new Vector2(from.x, i + 1),
				                                                    Quaternion.identity);
				tmp.transform.parent = graphics.transform;
			}

			// Make the top bit of the bar.
			GameObject top = (GameObject)GameObject.Instantiate(AssetDatabase.LoadAssetAtPath(fenceVBarTopPath, typeof(GameObject)),
			                                                       new Vector2(from.x, higher),
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