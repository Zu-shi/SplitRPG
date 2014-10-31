using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class HeightSorterScript : MonoBehaviour {

	public bool editModeRefresh = false;

	const float MAX_Z = 0;
	const float HEIGHT_FAC = 1000f;
	const float DRAWING_ORDER_FAC = 100f;
	//const float YCAM_FAC = 0.1f;
	const float YOFF = 0.1f;
	const float SLIGHT_DIFFERENCE = .01f;
	const float SLIGHTLY_MORE_DIFFERENCE = .02f;

	/*
	public float MaxZAtHeight(int height){
		return MAX_Z - height * HEIGHT_FAC - HEIGHT_FAC/5 + SLIGHT_DIFFERENCE;
	}

	public float MinZAtHeight(int height){
		return MaxZAtHeight (height + 1) + YOFF - SLIGHT_DIFFERENCE;
	}*/

	public void SetZForObject(HeightScript obj){

		CameraScript cam = null;

		if(obj.gameObject.layer == LayerMask.NameToLayer("Right")){
			cam = Globals.cameraRight;
		} else {
			cam = Globals.cameraLeft;
		}

		float heightOffs = HEIGHT_FAC * obj.height;
		float doOffs = DRAWING_ORDER_FAC * (int)obj.drawingOrder;

		//Try using fixed Y.

		float yOffs = (-obj.y ) * YOFF;
		/*
		if (obj.ys > 100) {
			Debug.Log (obj.y);
		}*/

		/*
		if ( Mathf.Abs(obj.y / obj.ys) > 5000) {
			Debug.LogWarning("Y value exceeds drawing order.");
		}*/

		float slightOffs = 0f;
		if (obj.slightlyAbove) {
			slightOffs -= SLIGHT_DIFFERENCE;
		}

		if (obj.slightlyBelow) {
			slightOffs += SLIGHT_DIFFERENCE;
		}

		if (obj.slightlyAbove) {
			slightOffs -= SLIGHTLY_MORE_DIFFERENCE;
		}
		
		if (obj.slightlyBelow) {
			slightOffs += SLIGHTLY_MORE_DIFFERENCE;
		}
		/*
		Debug.Log (heightOffs);
		Debug.Log (doOffs);
		Debug.Log (yOffs);*/
		//Debug.Log (MAX_Z + " - " + heightOffs + " - " + doOffs + "-" + yOffs);

		obj.z = MAX_Z - heightOffs - doOffs - yOffs + slightOffs;

		if(obj.z < cam.z){
			Debug.LogError("Sorting error: Object placed behind camera");
		}
		/*
		if(obj.z < MinZAtHeight(obj.height)){
			Debug.LogError("Sorting error: Object placed below z bounds");
		} else if (obj.z > MaxZAtHeight(obj.height)){
			Debug.LogError("Sorting error: Object placed above z bounds");
		}*/

	}

	void Update(){
		
		// When a person checks the check box, update the z value
		if(!Application.isPlaying){
			if(!editModeRefresh){
				return;
			} else {
				editModeRefresh = false;
				HeightScript[] heights = GameObject.FindObjectsOfType<HeightScript>();
				foreach(HeightScript hs in heights) {
					hs.editModeRefresh = true;
					hs.Update();
				}
			}
		}
	}


}
