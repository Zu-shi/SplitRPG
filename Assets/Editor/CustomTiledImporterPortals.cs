using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Tiled2Unity;

//[Tiled2Unity.CustomTiledImporter]
[Tiled2Unity.CustomTiledImporter(Order = short.MaxValue - 4)]
public class CustomPortalImporter : Tiled2Unity.ICustomTiledImporter {

	private List<string> senders = new List<string>();
	private List<string> receivers = new List<string>();
	private List<int> faders = new List<int>();
	private string pathPrefix = PrefabMapper.PrefabLocation;

	private GameObject senderPrefab;
	private GameObject receiverPrefab;
	private GameObject biPrefab;
	private GameObject loaderPrefab;
	
	private Dictionary<string, string> prefabMap;
	private string mapName;

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		Transform parent = null;
		if(gameObject != null){
			if(gameObject.transform != null){
				parent = gameObject.transform.parent;

				if(parent == null){
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
					//Debug.Log("MapName: " + mapName);
				}

			}
		}

		if(parent == null)
			return;
		if(senderPrefab == null || receiverPrefab == null || biPrefab == null) {
			//Debug.LogWarning("Temporary warning: right now only BidireactionalPortal has the updated assets.");
			biPrefab = AssetDatabase.LoadAssetAtPath(pathPrefix + mapName + "Portal1" + ".prefab", typeof(GameObject)) as GameObject;
			senderPrefab = AssetDatabase.LoadAssetAtPath(pathPrefix + mapName + "SendPortal2" + ".prefab", typeof(GameObject)) as GameObject;
			receiverPrefab = AssetDatabase.LoadAssetAtPath(pathPrefix + mapName + "ReceivePortal2" + ".prefab", typeof(GameObject)) as GameObject;
			//Debug.Log("TEST biPrefab: " + pathPrefix + mapName + "Portal1" + ".prefab");
			//Debug.Log("TEST senderPrefab: " + pathPrefix + mapName + "SendPortal2" + ".prefab");
			//Debug.Log("TEST receiverPrefab: " + pathPrefix + mapName + "ReceivePortal2" + ".prefab");

			//Debug.Log ("receiverPrefab " + receiverPrefab.name);
			//Debug.Log ("senderPrefab " + senderPrefab.name);
			//Debug.Log ("biPrefab " + biPrefab.name);

			//senderPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Portals/SendPortal.prefab", typeof(GameObject)) as GameObject;
			//receiverPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Portals/ReceivePortal.prefab", typeof(GameObject)) as GameObject;
			//biPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Portals/BidirectionalPortal.prefab", typeof(GameObject)) as GameObject;
			loaderPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TestObjects/Portals/LevelLoader.prefab", typeof(GameObject)) as GameObject;
		}

		if(parent.name.Contains("Unidirectional Portals") && props.ContainsKey("levelsToLoad")) {
			gameObject = MakePrefab(gameObject, loaderPrefab);
			LevelLoaderScript tmp = gameObject.GetComponent<LevelLoaderScript>();
			tmp.leftLevel = props["levelsToLoad"].Split(",".ToCharArray())[0];
			tmp.rightLevel = props["levelsToLoad"].Split(",".ToCharArray())[1];
			if(props.ContainsKey("canPush"))
				tmp.canPush = true;
			if(props.ContainsKey("canJump"))
				tmp.canJump = true;

			return;
		}
		if(parent.name.Contains("Unidirectional Portals") && props.ContainsKey("target")) {
			gameObject = MakePrefab(gameObject, senderPrefab);
			senders.Add(gameObject.name);
			receivers.Add(props["target"]);
			if(props.ContainsKey("fade")) {
				faders.Add(receivers.Count - 1);
			}
		}
		if(parent.name.Contains("Unidirectional Portals") && !props.ContainsKey("target")) {
			gameObject = MakePrefab(gameObject, receiverPrefab);
		}

		if(parent.name.Contains("Bidirectional Portals")) {
			//Debug.Log("Made bidirectional portal");
			gameObject = MakePrefab(gameObject, biPrefab);
			senders.Add(gameObject.name);
			receivers.Add(props["target"]);
		}
	}

	public void CustomizePrefab(GameObject prefab){
		for(int i = 0; i < senders.Count; i++) {
			GameObject sender = GameObject.Find(senders[i]);
			if(sender == null) {
				Debug.LogError("Could not find portal sender: " + senders[i]);
				continue;
			}
			if(sender.GetComponent<PortalSenderScript>() == null) {
				Debug.LogError("Sender does not have a portal sender script: " + senders[i]);
				continue;
			}

			GameObject receiver = GameObject.Find(receivers[i]);
			if(receiver == null) {
				Debug.LogError("Could not find portal receiver: " + receivers[i]);
				continue;
			}
			if(sender.GetComponent<PortalSenderScript>() == null) {
				Debug.LogError("Receiver does not have a portal receiver script: " + receivers[i]);
				continue;
			}

			sender.GetComponent<PortalSenderScript>().target = receiver.GetComponent<PortalReceiverScript>();

			if(faders.Contains(i)) {
				sender.GetComponent<PortalSenderScript>().fadeTransition = true;
			}
			//Debug.LogWarning("Portal connection: " + sender.name + ", " + receiver.name);
		}
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