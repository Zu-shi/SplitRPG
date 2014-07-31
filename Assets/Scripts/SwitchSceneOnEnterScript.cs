using UnityEngine;
using System.Collections;

public class SwitchSceneOnEnterScript : MonoBehaviour {

	public string sceneName;

	void Update () {
		bool collidingWithPlayer = Globals.collisionManager.IsPlayerCollidingWith(collider2D, gameObject.layer);
		if(collidingWithPlayer){
			Globals.gameManager.FadeOut(LoadLevel);
		}
	}

	void LoadLevel(){
		Application.LoadLevel(sceneName);
	}

}
