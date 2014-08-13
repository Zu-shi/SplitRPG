using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class HeightSorterScript : MonoBehaviour {

	public bool editModeRefresh = false;

	const float MAX_Z = 0;
	const float HEIGHT_FAC = 1f;
	const float DRAWING_ORDER_FAC = .1f;
	const float YCAM_FAC = .001f;
	const float SLIGHT_DIFFERENCE = .000001f;

	public float MaxZAtHeight(int height){
		return MAX_Z - height * HEIGHT_FAC + SLIGHT_DIFFERENCE;
	}

	public float MinZAtHeight(int height){
		return MaxZAtHeight (height + 1) + YCAM_FAC - SLIGHT_DIFFERENCE;
	}

	public void SetZForObject(HeightScript obj){

		CameraScript cam = null;

		if(obj.gameObject.layer == LayerMask.NameToLayer("Right")){
			cam = Globals.cameraRight;
		} else {
			cam = Globals.cameraLeft;
		}

		float heightOffs = HEIGHT_FAC * obj.height;
		float doOffs = DRAWING_ORDER_FAC * (int)obj.drawingOrder;
		float yOffs = YCAM_FAC * (cam.y + 20 - obj.y); // written so that the number will be a reasonable positive number

		// Objects off camera might try to make their yOffs too big or too small,
		// so we clamp it to reasonable values
		yOffs = Utils.Clamp(yOffs, 0, DRAWING_ORDER_FAC - YCAM_FAC); 
		float slightOffs = 0;
		if (obj.slightlyAbove) {
			slightOffs -= SLIGHT_DIFFERENCE;
		}

		if (obj.slightlyBelow) {
			slightOffs += SLIGHT_DIFFERENCE;
		}

		obj.z = MAX_Z - heightOffs - doOffs - yOffs + slightOffs;
		if(obj.z < cam.z){
			Debug.LogError("Sorting error: Object placed behind camera");
		}
		if(obj.z < MinZAtHeight(obj.height)){
			Debug.LogError("Sorting error: Object placed below z bounds");
		} else if (obj.z > MaxZAtHeight(obj.height)){
			Debug.LogError("Sorting error: Object placed above z bounds");
		}
		
		/*
			Debug.Log(obj.name);
			Debug.Log("Height: " + height);
			Debug.Log("Order: " + drawingOrder);
			Debug.Log("yOffset: " + yOffs);
			Debug.Log("Z: " + obj.z);
			*/
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
