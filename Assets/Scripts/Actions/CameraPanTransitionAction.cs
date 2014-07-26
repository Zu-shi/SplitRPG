using UnityEngine;
using System.Collections;

public class CameraPanTransitionAction : Action {

	CameraScript cameraScript;
	Vector2 transitionDest;
	float transitionSpeed;

	public static CameraPanTransitionAction Create(CameraScript c, Vector2 dest, float speed, Utils.VoidDelegate d){
		CameraPanTransitionAction a = Utils.CreateScript<CameraPanTransitionAction>();
		a.cameraScript = c;
		a.transitionDest = dest;
		a.transitionSpeed = speed;
		a.AddDelegate(d);
		return a;
	}

	public static CameraPanTransitionAction Create(CameraScript c, Vector2 dest, Utils.VoidDelegate d){
		return Create(c, dest, CameraScript.TRANSITION_SPEED, d);
	}

	void Update () {
		if(!started)
			return; 

		// Pan a bit towards the destination
		float dx = transitionDest.x;
		float dy = transitionDest.y;
		cameraScript.XYPanTo(dx, dy, transitionSpeed);
		
		// If we reached it we're done
		if(cameraScript.center.x == dx && cameraScript.center.y == dy){
			Finish();
		}
	}
}
