using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEditor;
using System.Collections;

//Purpose: Import Tiled maps with gates and buttons specified in the tech doc correctly into the game.
//Notes: the "visual" property of objects are optional, as long as a default prefab for buttons and gates are specified by the map.
//Author: Zuoming
[Tiled2Unity.CustomTiledImporter]
public class CustomTiledImporterSwitchesAndGates : Tiled2Unity.ICustomTiledImporter {
	
	private string pathPrefix = "Assets/Prefabs/MappedObjects/";
	private Dictionary<string, IDictionary<string, string>> switches = new Dictionary<string, IDictionary<string, string>>();
	private Dictionary<string, IDictionary<string, string>> gates = new Dictionary<string, IDictionary<string, string>>();
	private string defaultSwitchPrefabName;
	private string defaultGatePrefabName;
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props) {
		Transform parent = gameObject.transform.parent;
		if (parent == null) {
			if( props.ContainsKey("defaultSwitch") ){
				defaultSwitchPrefabName = props["defaultSwitch"];
			}
			if( props.ContainsKey("defaultSwitchGate") ){
				defaultGatePrefabName = props["defaultSwitchGate"];
			}
			return;
		}
		
		if(parent.name.Contains("Switches and Gates") ) {
			if(gameObject.name != ""){
				if( props.ContainsKey("target") ){
					//A button
					//Debug.LogWarning(gameObject.name);
					switches.Add(gameObject.name, props);
					//string[] targets = props["target"].Split(new string[] { ", " }, System.StringSplitOptions.None);
				}else{
					//This is a gate.
					/*
					if(gates.ContainsKey(gameObject.name)){
						gates.Remove(gameObject.name);
					}

					gates.Add (gameObject.name, props["visual"]);*/
					Debug.LogWarning(gameObject.name);
					gates.Add(gameObject.name, props);
					//Debug.LogWarning(gameObject.name);
				}
			}else{
				Debug.LogWarning("Object with empty name found in \"Switches and Gates\", skipping object.");
			}
		}
	}
	
	public void CustomizePrefab(GameObject prefab) {
		GameObject switchLayer;
		
		if (switchLayer = prefab.transform.FindChild ("Switches and Gates").gameObject) {
			
			foreach(KeyValuePair<string, IDictionary<string, string>> gate in gates)
			{
				GameObject gateObjParent = switchLayer.transform.FindChild(gate.Key).gameObject;
				GameObject gateObj;
				//Check if the gate has a default visual
				if( gate.Value.ContainsKey("visual") ){
					gateObj = generatePrefabUnderObject(gate.Value["visual"], gateObjParent);
				}else{
					gateObj = generatePrefabUnderObject(defaultGatePrefabName, gateObjParent);
				}

				if( gate.Value.ContainsKey("reverse") ){
					Debug.LogWarning(gate.Value["reverse"]);
					Debug.LogWarning(gateObj.GetComponent<GateScript>() != null);

					gateObj.GetComponent<GateScript>().reverse = bool.Parse( gate.Value["reverse"] );
				}
			}
			
			//This must happen after the gates importer, so that the correct gate objects can be found.
			foreach(KeyValuePair<string, IDictionary<string, string>> _switch in switches)
			{
				GameObject switchObjParent = switchLayer.transform.FindChild(_switch.Key).gameObject;
				GameObject switchObj;
				//Check if the button has a default visual
				if(_switch.Value.ContainsKey("visual")){
					switchObj = generatePrefabUnderObject(_switch.Value["visual"], switchObjParent);
				}else{
					switchObj = generatePrefabUnderObject(defaultSwitchPrefabName, switchObjParent);
				}
				
				SwitchScript ss = switchObj.GetComponent<SwitchScript>();
				Utils.assert(ss != null);
				Utils.assert(_switch.Value.ContainsKey("target"));
				
				string[] targets = _switch.Value["target"].Split(new string[] { ", " }, System.StringSplitOptions.None);
				foreach(string target in targets){
					GateScript gs = switchLayer.transform.Find(target + "Parent").Find (target).GetComponent<GateScript>();
					//gs.togglerToWatch = bs;
					ss.addGate( gs );
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
			item.layer = obj.layer;
		} else {
			Debug.LogWarning("Warning: prefab " + pathPrefix + prefabName + ".prefab" + " could not be found.");
		}
		return item;
	}
}
