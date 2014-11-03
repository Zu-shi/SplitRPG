using UnityEngine;
using System.Collections;

public class LatentHintManager : MonoBehaviour {

	public float countdownTimer = 0;
	public const float totalCountdown = 150f;

	public void resetTimer(){
		countdownTimer = totalCountdown;
	}
	
	public void accelerateTimer(){
		countdownTimer -= 25f;
	}

	// Use this for initialization
	void Start () {
		countdownTimer = totalCountdown;
	}
	
	// Update is called once per frame
	void Update () {
		countdownTimer -= Time.deltaTime;
		//Debug.Log (countdownTimer + " " + gameObject.name);
	}
}
