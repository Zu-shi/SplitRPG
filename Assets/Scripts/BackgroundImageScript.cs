using UnityEngine;
using System.Collections;

/// <summary>
/// Provides basic paralax functionality to an object in the scene.
/// </summary>
/// <author>Mark Gardner</author>
[ExecuteInEditMode]
public class BackgroundImageScript : _Mono {

	public float parralaxDepth = 1;
	public bool runInEditor = false;

	bool started = false;
	Vector3 lastCameraPosition;

	void Update () {
		if(!Application.isPlaying && !runInEditor){
			return;
		}

		CameraScript cameraScript = Globals.cameraLeft;
		if(Utils.LayerIs(gameObject.layer, "Right")){
			cameraScript = Globals.cameraRight;
		}

		if(!started){
			lastCameraPosition = cameraScript.transform.position;
			started = true;
		}

		if(cameraScript.transform.position != lastCameraPosition){
			Vector3 diff = cameraScript.transform.position - lastCameraPosition;
			float fac = - 1 / parralaxDepth;

			// localPosition should keep scaling things down from effecting the look too much
			transform.localPosition += fac * diff;
		}

		lastCameraPosition = cameraScript.transform.position;
	}


}
