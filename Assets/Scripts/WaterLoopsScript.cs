using UnityEngine;
using System.Collections;

public class WaterLoopsScript : MonoBehaviour {

	public Material[] m;
	public float speed;
	private float index = 0f;
	
	// Update is called once per frame
	void Update () {
		index += Time.deltaTime * speed * 30;
		GetComponent<MeshRenderer>().material = m[Mathf.FloorToInt(index) % m.Length];
		if(Mathf.FloorToInt(index - Time.deltaTime * speed) != Mathf.FloorToInt(index)){
			Debug.Log ("MeshRenderer Component is now " + Mathf.FloorToInt(index) % m.Length);
		}
	}
}
