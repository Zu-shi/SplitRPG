using UnityEngine;
using System.Collections.Generic;

public class HeightSorterScript : MonoBehaviour {

	const float MIN_Z = 0;
	const float HEIGHT_FAC = 1f;
	const float DRAWING_ORDER_FAC = .1f;
	const float YCAM_FAC = .001f;

	void Update(){

	}

	public void SetZForObject(HeightScript obj){
		CameraScript cam = Globals.cameraLeft;

		float heightOffs = HEIGHT_FAC * obj.height;
		float doOffs = DRAWING_ORDER_FAC * (int)obj.drawingOrder;
		float yOffs = YCAM_FAC * (cam.y + 20 - obj.y);
		
		obj.z = MIN_Z - (heightOffs + doOffs + yOffs);
		if(obj.z < cam.z){
			Debug.LogError("Sorting error: Object placed behind camera");
		}
		
		/*
			Debug.Log(obj.name);
			Debug.Log("Height: " + height);
			Debug.Log("Order: " + drawingOrder);
			Debug.Log("yOffset: " + yOffs);
			Debug.Log("Z: " + obj.z);
			*/
	}


}
