using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

//[Tiled2Unity.CustomTiledImporter]

[Tiled2Unity.CustomTiledImporter(Order = short.MaxValue - 3)]
class CustomTiledImporterSafelyRemoveParents : Tiled2Unity.ICustomTiledImporter{
	
	private List<GameObject>childsOfGameobject = new List<GameObject>();

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		
	}
	
	public void CustomizePrefab(GameObject prefab){
		
		GetAllChilds (prefab);
		
		foreach(GameObject gameObject in childsOfGameobject){
			if(gameObject.name.Contains("Parent")){
				gameObject.transform.GetChild(0).parent = gameObject.transform.parent;
				//Debug.LogWarning("Removed " + gameObject.name);
				Object.DestroyImmediate(gameObject);
			}
		}
	}
	
	private List<GameObject> GetAllChilds(GameObject transformForSearch)
	{
		List<GameObject> getedChilds = new List<GameObject>();
		
		foreach (Transform trans in transformForSearch.transform)
		{
			//Debug.Log (trans.name);
			GetAllChilds ( trans.gameObject );
			childsOfGameobject.Add ( trans.gameObject );            
		}       
		return getedChilds;
	}
}