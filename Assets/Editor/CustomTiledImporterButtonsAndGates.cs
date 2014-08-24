using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEditor;
using System.Collections;

//Purpose: Import Tiled maps with gates and buttons specified in the tech doc correctly into the game.
//Notes: the "visual" property of objects are optional, as long as a default prefab for buttons and gates are specified by the map.
//Author: Zuoming
[Tiled2Unity.CustomTiledImporter]
public class CustomTiledImporterButtonsSwitchesAndGates : Tiled2Unity.ICustomTiledImporter {


	private Dictionary<string, IDictionary<string, string>> buttons = new Dictionary<string, IDictionary<string, string>>();
	private Dictionary<string, IDictionary<string, string>> buttonGates = new Dictionary<string, IDictionary<string, string>>();
	private Dictionary<string, IDictionary<string, string>> switches = new Dictionary<string, IDictionary<string, string>>();
	private Dictionary<string, IDictionary<string, string>> gates = new Dictionary<string, IDictionary<string, string>>();
	private Dictionary<string, string> prefabMap;
	private string mapName;

	private bool autobond;

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
			
			if(props.ContainsKey("autobond")){
				autobond = bool.Parse(props["autobond"]);
			}else{
				autobond = false;
			}
			return;
		}

		if(parent.name.Contains("Buttons and Gates") ) {
			if(gameObject.name != ""){
				if( props.ContainsKey("target") ){
					//A button
					//Debug.LogWarning(gameObject.name);
					buttons.Add(gameObject.name, props);
				}else{
					//Debug.LogWarning(gameObject.name);
					buttonGates.Add(gameObject.name, props);
					//This is a gate that has specified a specific visual.
				}
			}else{
				Debug.LogWarning("Object with empty name found in \"Switches and Gates\", skipping object.");
			}
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
		GameObject buttonLayer = null;
		GameObject switchLayer = null;;
		if (prefab.transform.GetChild (0).FindChild ("Buttons and Gates") != null) {
			buttonLayer = prefab.transform.GetChild (0).FindChild ("Buttons and Gates").gameObject;
		}
		if (prefab.transform.GetChild(0).FindChild ("Switches and Gates") != null ) {
			switchLayer = prefab.transform.GetChild(0).FindChild ("Switches and Gates").gameObject;
		}
		if (buttonLayer) {

			//This must happen after the gates importer, so that the correct gate objects can be found.
			foreach(KeyValuePair<string, IDictionary<string, string>> button in buttons){
				GameObject buttonObjParent = buttonLayer.transform.FindChild(button.Key).gameObject;
				GameObject buttonObj;

				//Check if the button has a default visual
				string visualName;
				if(button.Value.ContainsKey("visual")){
					//Debug.Log ("Contains visual: " + mapName + prefabMap[button.Value["visual"]] );
					visualName = prefabMap[button.Value["visual"].ToLower()];
				}else{
					//Debug.Log ("Does not contain visual: " + mapName + prefabMap["button1"] );
					visualName = prefabMap["button1"];
				}
				buttonObj = generatePrefabUnderObject(mapName + visualName, buttonObjParent);

				ButtonScript bs = buttonObj.GetComponent<ButtonScript>();
				Utils.assert(bs != null);
				Utils.assert(button.Value.ContainsKey("target"));

				string[] targets = button.Value["target"].Split(new string[] { ", " }, System.StringSplitOptions.None);
				foreach(string target in targets){
					Debug.Log (visualName);
					GateScript gs = ImportGate(target, buttonGates[target], buttonLayer, PrefabMapper.activatorToGateMap[visualName]);
					bs.addGate( gs );
				}
			
				if( button.Value.ContainsKey("time") ){
					bs.timerLength = float.Parse(button.Value["time"]);
				}else{
					bs.timerLength = 0;
				}
			}

		}

		if (switchLayer) {
			
			//This must happen after the gates importer, so that the correct gate objects can be found.
			foreach(KeyValuePair<string, IDictionary<string, string>> _switch in switches)
			{
				GameObject switchObjParent = switchLayer.transform.FindChild(_switch.Key).gameObject;
				GameObject switchObj;
				//Check if the button has a default visual
				string visualName;
				if(_switch.Value.ContainsKey("visual")){
					visualName = prefabMap[_switch.Value["visual"].ToLower()];
				}else{
					visualName = prefabMap["switch1"];
				}
				switchObj = generatePrefabUnderObject(mapName + visualName, switchObjParent);

				SwitchScript ss = switchObj.GetComponent<SwitchScript>();
				Utils.assert(ss != null);
				Utils.assert(_switch.Value.ContainsKey("target"));
				
				string[] targets = _switch.Value["target"].Split(new string[] { ", " }, System.StringSplitOptions.None);
				foreach(string target in targets){
					Debug.Log (visualName);
					GateScript gs = ImportGate(target, gates[target], switchLayer, PrefabMapper.activatorToGateMap[visualName]);
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

			itemMono.x += itemMono.xs;
			itemMono.y -= itemMono.ys;

			item.transform.parent = obj.transform;
			//Debug.LogWarning("Object " + obj.name + " added.");
			item.name = obj.name;
			obj.name = obj.name + "Parent";
			item.layer = obj.layer;
		} else {
			Debug.LogWarning("Warning: prefab " + prefabName + " could not be found.");
		}
		return item;
	}

	//
	private GateScript ImportGate(string gate, IDictionary<string, string> gateProperties, GameObject layer, string defaultPrefab){
		GameObject gateObjParent = layer.transform.FindChild(gate).gameObject;
		GameObject gateObj;
		//Check if the gate has a default visual
		if( gateProperties.ContainsKey("visual") ){
			gateObj = generatePrefabUnderObject(mapName + prefabMap[gateProperties["visual"]], gateObjParent);
		}else{
			gateObj = generatePrefabUnderObject(mapName + prefabMap[defaultPrefab], gateObjParent);
		}
		
		EdgeCollider2D ec = gateObjParent.GetComponent<EdgeCollider2D>();
		if (ec.points[0].y != ec.points[1].y){
			gateObj.GetComponent<GateScript>().horizontal = false;
			gateObj.GetComponent<SpriteRenderer>().sprite = gateObj.GetComponent<GateScript>().closedSpriteV;
			gateObj.transform.position += new Vector3(-1,0,0);
			RotateEdgeCollider(gateObj.GetComponent<EdgeCollider2D>());
		}
		else {
			
			gateObj.transform.position += new Vector3(0,1.5f,0);
		}
		
		if( gateProperties.ContainsKey("reverse") ){
			gateObj.GetComponent<GateScript>().reverse = bool.Parse( gateProperties["reverse"] );
		}

		return gateObj.GetComponent<GateScript> ();
	}

	private void RotateEdgeCollider(EdgeCollider2D ec){
		if (ec.pointCount == 2) {
			float xavg = (ec.points[0].x + ec.points[1].x) / 2;
			ec.points = new Vector2[] {new Vector2(xavg, ec.points[0].y + Mathf.Abs(ec.points[0].x - xavg) ), 
					new Vector2(xavg, ec.points[0].y - Mathf.Abs(ec.points[0].x - xavg)) } ;
		} else {
			Debug.LogWarning("Gate with point count above or less than 2 not supported.");
		}
	}
	/*
	private void ImportGates(Dictionary<string, IDictionary<string, string>> gates, GameObject layer, string gatename){
		foreach(KeyValuePair<string, IDictionary<string, string>> gate in gates)
		{
			GameObject gateObjParent = layer.transform.FindChild(gate.Key).gameObject;
			GameObject gateObj;
			//Check if the gate has a default visual
			if( gate.Value.ContainsKey("visual") ){
				gateObj = generatePrefabUnderObject(mapName + prefabMap[gate.Value["visual"]], gateObjParent);
			}else{
				gateObj = generatePrefabUnderObject(mapName + prefabMap[gatename], gateObjParent);
			}
			
			EdgeCollider2D ec = gateObjParent.GetComponent<EdgeCollider2D>();
			if (ec.points[0].y != ec.points[1].y){
				gateObj.GetComponent<GateScript>().horizontal = false;
				gateObj.GetComponent<SpriteRenderer>().sprite = gateObj.GetComponent<GateScript>().closedSpriteV;
				gateObj.transform.position += new Vector3(-1,0,0);
			}
			else {
				
				gateObj.transform.position += new Vector3(0,1.5f,0);
			}
			
			if( gate.Value.ContainsKey("reverse") ){
				gateObj.GetComponent<GateScript>().reverse = bool.Parse( gate.Value["reverse"] );
			}

		}
	}*/

}
