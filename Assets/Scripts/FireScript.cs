using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour {

	public int level = 0;
	
	// Update is called once per frame
	void Update () {
		if(Globals.fireLevel == level - 1){
			gameObject.SetActive(false);
		}
	}
}
