/*
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

	private Dictionary<string, IDictionary<string, string>> switches = new Dictionary<string, IDictionary<string, string>>();
	private Dictionary<string, IDictionary<string, string>> gates = new Dictionary<string, IDictionary<string, string>>();
	private Dictionary<string, string> prefabMap;
	private string mapName;
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props) {
		Transform parent = gameObject.transform.parent;
		if (parent == null) {
			if(props.ContainsKey("map")){
				prefabMap = PrefabMapper.maps[props["map"]];
				mapName = props["map"] + "/";
			}else{
				prefabMap = PrefabMapper.maps["default"];
				mapName = "";
			}
			return;
		}
		
		if(parent.name.Contains("Switches and Gates") ) {
			if(gameObject.name != ""){
				if( props.ContainsKey("target") ){
					//A button
					switches.Add(gameObject.name, props);
				}else{
					//This is a gate.
					gates.Add(gameObject.name, props);
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
					gateObj = generatePrefabUnderObject(mapName + prefabMap[gate.Value["visual"]], gateObjParent);
				}else{
					gateObj = generatePrefabUnderObject(mapName + prefabMap["switch1gate"], gateObjParent);
				}

				if( gate.Value.ContainsKey("reverse") ){

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
					switchObj = generatePrefabUnderObject(mapName + prefabMap[_switch.Value["visual"]], switchObjParent);
				}else{
					switchObj = generatePrefabUnderObject(mapName + prefabMap["switch1"], switchObjParent);
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
		GameObject item = AssetDatabase.LoadAssetAtPath(PrefabMapper.PrefabLocation + prefabName + ".prefab", typeof(GameObject)) as GameObject;
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
			Debug.LogWarning("Warning: prefab " + PrefabMapper.PrefabLocation + prefabName + ".prefab" + " could not be found.");
		}
		return item;
	}
}
<<<<<<< HEAD:Assets/Editor/CustomTiledImporterSwitchesAndGates.cs
*/
