using UnityEngine;
using System.Collections;

public class ColorShiftScript : MonoBehaviour {

	private float hue = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		hue += 0.0006f;
		if (hue > 1f) {
			hue -= 1f;
		}

		Color c = Color.blue;
		if(GetComponent<MeshRenderer> () != null){
			c = GetComponent<MeshRenderer> ().material.color;
		}else if(GetComponent<SpriteRenderer> () != null){
			c = GetComponent<SpriteRenderer> ().material.color;
		}

		//EditorGUIUtility.HSVToRGB (hue, 1, 1);
		HSBColor hsl = new HSBColor(hue, 1.0f, 1.0f, c.a);
		Color rgbCol = hsl.ToColor();

		//Debug.Log ( rgbCol.r + " " + rgbCol.g + " " + rgbCol.b );
		if(GetComponent<MeshRenderer> () != null){
			GetComponent<MeshRenderer> ().material.color = rgbCol;
		}else if(GetComponent<SpriteRenderer> () != null){
			GetComponent<SpriteRenderer> ().material.color = rgbCol;
		}
	}
}
