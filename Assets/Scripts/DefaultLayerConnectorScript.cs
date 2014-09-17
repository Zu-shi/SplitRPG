using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;

//[ExecuteInEditMode]
public class DefaultLayerConnectorScript : MonoBehaviour {

	//This script assumes that Switches and Gates that are shared across two screens are in the Switches and Gates(Default) layer,
	//Following the same conventions as Switches and Gates.

	/*
	[Tooltip("The first prefab to merge. Autofilled as secondmost recent by importer script for default switches.")]
	public GameObject prefab1 = null;

	[Tooltip("The second prefab to merge. Autofilled as secondmost recent by importer script for default switches.")]
	public GameObject prefab2 = null;
	
	public bool mergePrefabs = false;

	void Update () {
		if(!mergePrefabs){
			return;
		} else {
			mergePrefabs = false;
		}
		LinkPushblocks();
		LinkSwitches();
	}*/

	void LinkPushblocks(GameObject prefab1,GameObject prefab2){
		//Connect pushblocks by deleting the second copy.
		List<Vector2> toDeleteAtPosition = new List<Vector2> ();
		Transform pushblocksLayerTransform = Utils.FindChildRecursive(prefab1, "Pushblocks(Default)");
		Transform pushblocksLayerTransform2 = Utils.FindChildRecursive(prefab2, "Pushblocks(Default)");
		foreach (Transform pb in pushblocksLayerTransform) {
			PushBlockColliderScript pbs = pb.gameObject.GetComponent<PushBlockColliderScript>();
			Utils.assert(pbs != null, "Check for BlockColliderScript in the Pushblocks(Default) layer for object " + pb.gameObject.name);
			Debug.Log ("Found switch " + pb.name);
			bool foundLink = false;

			foreach (Transform pb2 in pushblocksLayerTransform2) {
				PushBlockColliderScript pbs2 = pb2.gameObject.GetComponent<PushBlockColliderScript>();
				Utils.assert(pbs2 != null, "Check for BlockColliderScript in the Pushblocks(Default) layer for object " + pb2.gameObject.name);
				if(pb2.position.Equals(pb.position)){
					Debug.Log ("Found counterpart " + pb.name);
					foundLink = true;
					toDeleteAtPosition.Add(new Vector2(pbs2.x, pbs2.y));
				}
			}
			if(!foundLink){Debug.Log ("Cannot find counterpart for " + prefab1.name + " pushblock at " + pbs.x + ", " + pbs.y + ".");}
		}

		foreach (Vector2 pos in toDeleteAtPosition) {
			GameObject toDestroy = null;
			foreach (Transform pb2 in pushblocksLayerTransform2) {
				PushBlockColliderScript pbs2 = pb2.gameObject.GetComponent<PushBlockColliderScript>();
				if(pbs2.x == pos.x && pbs2.y == pos.y){
					toDestroy = pbs2.gameObject;
					break;
				}
			}
			GameObject.DestroyImmediate (toDestroy, true);
		}
	}
	
	//This function looks ugly, clean it later.
	void LinkSwitches(GameObject prefab1,GameObject prefab2){
		//Connect switches by sharing the toggler to watch.
		Transform switchesLayerTransform = Utils.FindChildRecursive(prefab1, "Switches and Gates(Default)");
		Transform switchesLayerTransform2 = Utils.FindChildRecursive(prefab2, "Switches and Gates(Default)");
		foreach (Transform s in switchesLayerTransform) {
			SwitchScript ss;
			if((ss = s.gameObject.GetComponent<SwitchScript>()) != null){
				Debug.Log ("Found switch " + s.name);
				foreach (Transform s2 in switchesLayerTransform2) {
					Debug.Log(s.name + " " + s2.name);
					SwitchScript ss2;
					if(s2.name == s.name){
						if((ss2 = s2.gameObject.GetComponent<SwitchScript>()) != null){
							Debug.Log ("Found counterpart " + s.name);
							Debug.Log ("Check not actuall the SAME " + (ss2 == ss));
							ss2.twin = ss;
							ss.twin = ss2;
							ss2._toggler = ss._toggler;
							//Debug.Log (ss2._toggler == ss._toggler);
						}else{
							Debug.LogWarning ("Switch that shares a name does not have a switch script.");
						}
					}
				}
			}
		}
	}

	/*
	void HookSwitchesToGates (GameObject switchesMap, GameObject gatesMap) {

		Transform switchesLayerTransform = Utils.FindChildRecursive(switchesMap, "Switches and Gates(Default)");
		Transform gatesLayerTransform = Utils.FindChildRecursive(gatesMap, "Switches and Gates(Default)");

		foreach (Transform s in switchesLayerTransform) {
			//This is not an efficient implementation, but under most conditions it will process little data.
			SwitchScript ss;

			if((ss = s.gameObject.GetComponent<SwitchScript>()) != null){
				Debug.Log ("Found switch " + s.name);
				if(ss.remainingGates != ""){
					string[] targets = ss.remainingGates.Split(new string[] { ", " }, System.StringSplitOptions.None);
					foreach(string target in targets){
						GateScript gs = FindGate(gatesLayerTransform, target);
						if(gs != null){
							ss.addGate(gs);
							Debug.Log(s.name + " hooked to " + gs.gameObject.name);
						}
					}

				}else{
					Debug.Log ("No remaining gates to be found for " + ss.gameObject.name);
				}
			}
		}

	}

	GateScript FindGate(Transform gatesLayerTransform, string name){
		foreach (Transform g in gatesLayerTransform) {
			if(g.gameObject.name == "name"){
				if(g.gameObject.GetComponent<GateScript>() != null){
					return g.gameObject.GetComponent<GateScript>();
				}else{
					Debug.LogWarning ("Gate " + name + " found but has no gateScript attatched.");
					return null;
				}
			}
		}
		Debug.LogWarning ("Gate " + name + " could not be found.");
		return null;
	}*/
}
