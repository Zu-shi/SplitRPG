using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
public class CustomPortalImporter : Tiled2Unity.ICustomTiledImporter {

	private List<string> senders = new List<string>();
	private List<string> receivers = new List<string>();

	private GameObject senderPrefab;
	private GameObject receiverPrefab;
	private GameObject biPrefab;
	private GameObject loaderPrefab;
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		Transform parent = gameObject.transform.parent;
		if(parent == null)
			return;
		if(senderPrefab == null || receiverPrefab == null || biPrefab == null) {
			senderPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Portals/SendPortal.prefab", typeof(GameObject)) as GameObject;
			receiverPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Portals/ReceivePortal.prefab", typeof(GameObject)) as GameObject;
			biPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Portals/BidirectionalPortal.prefab", typeof(GameObject)) as GameObject;
			loaderPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Portals/LevelLoader.prefab", typeof(GameObject)) as GameObject;
		}

		if(parent.name.Contains("Unidirectional Portals") && props.ContainsKey("levelsToLoad")) {
			gameObject = MakePrefab(gameObject, loaderPrefab);
			LevelLoaderScript tmp = gameObject.GetComponent<LevelLoaderScript>();
			tmp.leftLevel = props["levelsToLoad"].Split(",".ToCharArray())[0];
			tmp.rightLevel = props["levelsToLoad"].Split(",".ToCharArray())[1];
			return;
		}
		if(parent.name.Contains("Unidirectional Portals") && props.ContainsKey("target")) {
			gameObject = MakePrefab(gameObject, senderPrefab);
			senders.Add(gameObject.name);
			receivers.Add(props["target"]);
		}
		if(parent.name.Contains("Unidirectional Portals") && !props.ContainsKey("target")) {
			gameObject = MakePrefab(gameObject, receiverPrefab);
		}

		if(parent.name.Contains("Bidirectional Portals")) {
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