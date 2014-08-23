using UnityEngine;
using System.Collections;

public class ColorShiftScript : MonoBehaviour {

	public float hue = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		hue += 0.01f;
		if (hue > 1f) {
			hue -= 1f;
		}

		//EditorGUIUtility.HSVToRGB (hue, 1, 1);
		HSBColor hsl = new HSBColor(hue, 1.0f, 1.0f);
		Color rgbCol = hsl.ToColor();
		//Debug.Log ( rgbCol.r + " " + rgbCol.g + " " + rgbCol.b );
		if(GetComponent<MeshRenderer> () != null){
			GetComponent<MeshRenderer> ().material.color = rgbCol;
		}else if(GetComponent<SpriteRenderer> () != null){
			GetComponent<SpriteRenderer> ().material.color = rgbCol;
		}
	}
}
