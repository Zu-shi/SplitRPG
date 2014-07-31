using UnityEngine;
using System.Collections;

public class EndScreenScript : MonoBehaviour {

	bool faded;
	FaderScript fader;

	// Use this for initialization
	void Start () {
		fader = GameObject.Find("Fader").GetComponent<FaderScript>();
		fader.guiAlpha = 1f;
		fader.FadeUp(null);

		faded = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!faded){
			if(Input.GetMouseButton(0) ||
			   Input.GetKeyDown("escape") || 
//			   Input.GetKeyDown("up") || 
//			   Input.GetKeyDown("down") || 
//			   Input.GetKeyDown("left") || 
//			   Input.GetKeyDown("right") || 
			   Input.GetKeyDown("enter") || 
			   Input.GetKeyDown("space")){

				fader.FadeDown(EndGame);
				faded = true;
			}
		}
	}

	void EndGame(){
		Debug.Log ("Quitting game");
		Application.Quit();
	}


}
