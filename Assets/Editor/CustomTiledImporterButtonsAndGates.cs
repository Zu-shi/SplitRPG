using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEditor;
using System.Collections;

//Purpose: Import Tiled maps with gates and buttons specified in the tech doc correctly into the game.
//Notes: the "visual" property of objects are optional, as long as a default prefab for buttons and gates are specified by the map.
//Author: Zuoming
[Tiled2Unity.CustomTiledImporter]
public class CustomTiledImporterButtonsAndGates : Tiled2Unity.ICustomTiledImporter {
	
	private string pathPrefix = "Assets/Prefabs/MappedObjects/";
	private static Dictionary<string, IDictionary<string, string>> buttons = new Dictionary<string, IDictionary<string, string>>();
	private static Dictionary<string, string> gates = new Dictionary<string, string>();
	private string defaultButtonPrefabName;
	private string defaultGatePrefabName;

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props) {
		Transform parent = gameObject.transform.parent;
		if (parent == null) {
			if( props.ContainsKey("defaultButton") ){
				defaultButtonPrefabName = props["defaultButton"];
			}
			if( props.ContainsKey("defaultButtonGate") ){
				defaultGatePrefabName = props["defaultButtonGate"];
			}
			return;
		}

		if(parent.name.Contains("Buttons and Gates") ) {
			if( props.ContainsKey("target") ){
				//A button
				//Debug.LogWarning(gameObject.name);
				buttons.Add(gameObject.name, props);
				string[] targets = props["target"].Split(new string[] { ", " }, System.StringSplitOptions.None);

				foreach(string target in targets){
					gates.Add (target, "");
				}
			}else{
				//This is a gate that has specified a specific visual.
				gates.Remove(gameObject.name);
				gates.Add (gameObject.name, props["visual"]);
			}
		}
	}
	
	public void CustomizePrefab(GameObject prefab) {
		GameObject buttonLayer;
		
		if (buttonLayer = prefab.transform.FindChild ("Buttons and Gates").gameObject) {

			foreach(KeyValuePair<string, string> gate in gates)
			{
				GameObject gateObjParent = buttonLayer.transform.FindChild(gate.Key).gameObject;
				//Check if the gate has a default visual
				if(gate.Value != ""){
					generatePrefabUnderObject(gate.Value, gateObjParent);
				}else{
					generatePrefabUnderObject(defaultGatePrefabName, gateObjParent);
				}
			}

			//This must happen after the gates importer, so that the correct gate objects can be found.
			foreach(KeyValuePair<string, IDictionary<string, string>> button in buttons)
			{
				GameObject buttonObjParent = buttonLayer.transform.FindChild(button.Key).gameObject;
				GameObject buttonObj;
				//Check if the button has a default visual
				if(button.Value.ContainsKey("visual")){
					buttonObj = generatePrefabUnderObject(button.Value["visual"], buttonObjParent);
				}else{
					buttonObj = generatePrefabUnderObject(defaultButtonPrefabName, buttonObjParent);
				}

				ButtonScript bs = buttonObj.GetComponent<ButtonScript>();
				Utils.assert(bs != null);
				Utils.assert(button.Value.ContainsKey("target"));

				string[] targets = button.Value["target"].Split(new string[] { ", " }, System.StringSplitOptions.None);
				foreach(string target in targets){
					GateScript gs = buttonLayer.transform.Find(target + "Parent").Find (target).GetComponent<GateScript>();
					//gs.togglerToWatch = bs;
					bs.addGate( gs );
				}

				if( button.Value.ContainsKey("time") ){
					bs.timerLength = float.Parse(button.Value["time"]);
				}else{
					bs.timerLength = 0;
				}
			}

		}
	}

	public GameObject generatePrefabUnderObject(string prefabName, GameObject obj){
		GameObject item = AssetDatabase.LoadAssetAtPath(pathPrefix + prefabName + ".prefab", typeof(GameObject)) as GameObject;
		if (item != null) {
			//Add Vector3.one to offset center differences.
			item = (GameObject)GameObject.Instantiate (item, obj.transform.position, obj.transform.rotation);

			_Mono itemMono = item.GetComponent<_Mono> ();
			itemMono.xs /= Utils.TILED_TO_UNITY_SCALE;
			itemMono.ys /= Utils.TILED_TO_UNITY_SCALE;
			itemMono.x += itemMono.xs / 2;
			itemMono.y -= itemMono.ys / 2;
			item.transform.parent = obj.transform;
			//Debug.LogWarning("Object " + obj.name + " added.");
			item.name = obj.name;
			obj.name = obj.name + "Parent";
		} else {
			//Debug.LogWarning("Warning: prefab " + prefabName + " could not be found.");
		}
		return item;
	}
}
