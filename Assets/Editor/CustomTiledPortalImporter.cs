using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
public class CustomPortalImporter : Tiled2Unity.ICustomTiledImporter {

	private static List<string> senders = new List<string>();
	private static List<string> receivers = new List<string>();
	
	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if(gameObject.transform.parent.name.Contains("UnidirectionalPortals") && props.ContainsKey("target")) {
			GetSender(gameObject);
			senders.Add(gameObject.name);
			receivers.Add(props["target"]);
		}
		if(gameObject.transform.parent.name.Contains("UnidirectionalPortals") && !props.ContainsKey("target")) {
			GetReceiver(gameObject);
		}
		if(gameObject.transform.parent.name.Contains("BidirectionalPortals")) {
			GetReceiver(gameObject);
			GetSender(gameObject);
			senders.Add(gameObject.name);
			receivers.Add(props["target"]);
		}
	}

	public void CustomizePrefab(GameObject prefab){
		for(int i = 0; i < senders.Count; i++) {
			GameObject.Find(senders[i]).GetComponent<PortalSenderScript>().target = GameObject.Find(receivers[i]).GetComponent<PortalReceiverScript>();
		}
	}

	private PortalSenderScript GetSender(GameObject o){
		PortalSenderScript ps = o.GetComponent<PortalSenderScript>();
		if(ps == null){
			ps = o.AddComponent<PortalSenderScript>();
		}
		return ps;
	}

	private PortalReceiverScript GetReceiver(GameObject o){
		PortalReceiverScript ps = o.GetComponent<PortalReceiverScript>();
		if(ps == null){
			ps = o.AddComponent<PortalReceiverScript>();
		}
		return ps;
	}
}