using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

//[Tiled2Unity.CustomTiledImporter]

[Tiled2Unity.CustomTiledImporter(Order = short.MaxValue - 1)]
class CustomTiledImporterHeight : Tiled2Unity.ICustomTiledImporter{

	private List<GameObject>childsOfGameobject = new List<GameObject>();

	private Dictionary<string, int> heightMap = new Dictionary<string, int>{
		{ "Objects(Invisible)", 1 },
		{ "Objects(Invisible) 2", 2 },
		{ "Switches and Gates", 1 },
		{ "Switches and Gates 2", 2 },
		{ "Buttons and Gates", 1 },
		{ "Buttons and Gates 2", 2 },
		{ "Bidirectional Portals", 1 },
		{ "Bidirectional Portals 2", 2 },
		{ "Unidirectional Portals", 1 },
		{ "Unidirectional Portals 2", 2 },
		{ "Pushblocks(Default)", 1 }
	};
	
	private Dictionary<string, DrawingOrder> orderMap = new Dictionary<string, DrawingOrder>{
		{ "Objects(Invisible)", DrawingOrder.OBJECTS },//,
		{ "Pushblocks(Default)", DrawingOrder.OBJECTS },//,
		{ "Bidirectional Portals", DrawingOrder.ON_GROUND },//,
		{ "Unidirectional Portals", DrawingOrder.ON_GROUND }
		//{ "Switches and Gates", DrawingOrder.OBJECTS },
		//{ "Buttons and Gates", DrawingOrder.OBJECTS }
	};
	private const int DEFAULT_HEIGHT = 1;


	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){

		if (props.ContainsKey ("height")) {
			//Debug.Log (gameObject.name);
			HeightScript hs = Utils.GetHeightScript (gameObject);
			hs.height = int.Parse(props["height"]);
		}

		
		if (props.ContainsKey ("order")) {
			//Debug.Log (gameObject.name);
			HeightScript hs = Utils.GetHeightScript (gameObject);
			if(props["order"].ToLower() == "above"){
				hs.slightlyAbove = true;
			}else if(props["order"].ToLower() == "below"){
				hs.slightlyBelow = true;
			}else{
				Debug.LogWarning("Uknown order " + props["order"] + ", only \"above\" or \"below\" recognized");
			}
		}
	}

	public void CustomizePrefab(GameObject prefab){
		
		GetAllChilds (prefab);

		foreach(GameObject gameObject in childsOfGameobject){
			if (gameObject.transform.parent == null) {
				return;
			}
			
			foreach(KeyValuePair<string, int> entry in heightMap){
				if (gameObject.transform.parent.name.Equals (entry.Key)) {
					HeightScript hs = Utils.GetHeightScript(gameObject);
					hs.height = entry.Value;
				}
			}
			
			foreach(KeyValuePair<string, DrawingOrder> entry in orderMap){
				if (gameObject.transform.parent.name.Contains (entry.Key)) {
					HeightScript hs = Utils.GetHeightScript(gameObject);
					hs.drawingOrder = entry.Value;
				}
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