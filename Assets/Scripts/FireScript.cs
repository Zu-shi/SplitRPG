using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour {

	public int level = 0;
	
	// Update is called once per frame
	void Update () {

		// prevents bugs
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0);

		if(Globals.fireLevel == level - 1){
			gameObject.SetActive(false);
		}
	}
}
