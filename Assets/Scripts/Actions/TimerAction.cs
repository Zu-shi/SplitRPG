using UnityEngine;
using System.Collections;

public class TimerAction : Action {

	float timeLeft;

	public static TimerAction Create(float t, Utils.VoidDelegate callback){
		TimerAction a = Utils.CreateScript<TimerAction>();
		a.AddDelegate(callback);
		a.timeLeft = t;
		return a;
	}

	void Update () {
		if(started){
			timeLeft -= Time.deltaTime;
			if(timeLeft <= 0)
				Finish();
		}
	}
}
