using UnityEngine;
using System.Collections;

public class CreditsTextScript : MonoBehaviour {

	public GameObject textBox;
	public string[] contents;

	public float fadeRate = .05f;
	public float sustain = 5f;
	public float delay = .5f;


	TextMesh textMesh;

	int currStep;
	int fadeDir = 0;

	float alpha {
		get{
			return textMesh.color.a;
		} 
		set{
			textMesh.color = new Vector4(1, 1, 1, value);
		}
	}

	// Use this for initialization
	void Start () {
		// convert the string \n to the character \n
		for(int i = 0; i < contents.Length; i++){
			contents[i] = contents[i].Replace("\\n", "\n");
		}

		currStep = -1;

		textMesh = textBox.GetComponent<TextMesh>();

		FadeInNext();

		alpha = 0;
	}

	void NextStep(){
		Debug.Log ("Stepping " + currStep);

		currStep++;
		if(currStep < contents.Length){
			textMesh.text = contents[currStep];
			Debug.Log ("New text = " + contents[currStep] + " = " + textMesh.text);
		} else
			textMesh.text = "";

	}

	void FadeOut(){
		fadeDir = -1;
	}

	void FadeInNext(){
		NextStep();
		fadeDir = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(fadeDir == 1){
			alpha += fadeRate;
			if(alpha >= 1){
				alpha = 1;
				fadeDir = 0;
				Invoke ("FadeOut", sustain);
			}
		} else if (fadeDir == -1){
			alpha -= fadeRate;
			if(alpha <= 0){
				alpha = 0;
				fadeDir = 0;
				Invoke ("FadeInNext", delay);
			}
		}
	}
}
