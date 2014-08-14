using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HeightScript : _Mono {

	public DrawingOrder drawingOrder;
	public int height = 1;
	public bool slightlyAbove = false;
	public bool slightlyBelow = false;

	public bool editModeRefresh = false;

	void Start(){
		editModeRefresh = false;
	}

	public void Update(){

		// When a person checks the check box, update the z value
		if(!Application.isPlaying){
			if(!editModeRefresh){
				return;
			} else {
				editModeRefresh = false;
			}
		} 

		/*
		if (gameObject.name == "pushblock") {
			Debug.LogWarning("pushblock updating.");
		}*/
		Globals.heightSorter.SetZForObject(this);
	}
}
