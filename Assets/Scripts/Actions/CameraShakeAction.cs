using UnityEngine;
using System.Collections;

public class CameraShakeAction : Action {

	CameraScript cameraScript;
	Vector2 shakeDest;
	float shakeSpeed;
	float duration;

	public static CameraShakeAction Create(CameraScript cam, float duration, Utils.VoidDelegate callback){
		CameraShakeAction a = Utils.CreateScript<CameraShakeAction>();
		a.cameraScript = cam;
		a.duration = duration;
		a.AddDelegate(callback);
		return a;
	}

	void Start(){
		shakeSpeed = 16f;
		shakeDest = new Vector2(.14f, 0);
	}

	void StartNextShake(){
		// Right now we just move the camera back and forth really fast
		shakeDest = new Vector2(-shakeDest.x, shakeDest.y);
	}

	void Update () {
		if(!started)
			return; 

		duration -= Time.deltaTime;
		if(duration <= 0){
			Finish();
			return;
		}

		// Start the next shake if we reached shakeDest
		if(cameraScript.offset.x == shakeDest.x && cameraScript.offset.y == shakeDest.y){
			StartNextShake();
		}
		
		// Move the offset towards shakeDest
		float ss = shakeSpeed * Time.deltaTime;
		float tx, ty;
		tx = Utils.MoveValueTowards(cameraScript.offset.x, shakeDest.x, ss);
		ty = Utils.MoveValueTowards(cameraScript.offset.y, shakeDest.y, ss);
		cameraScript.offset = new Vector2(tx, ty);
	}
}
