using UnityEngine;
using System.Collections;

public class ManualCheckpointScript : MonoBehaviour {

	public bool save = false;
	public bool load = false;
	
	// Update is called once per frame
	void Update () {
		if(save){
			save = false;
			Globals.levelManager.SaveCheckpoint();
			return;
		}
		if(load) {
			load = false;
			Globals.levelManager.LoadLastCheckpoint();
		}
	
	}
}
