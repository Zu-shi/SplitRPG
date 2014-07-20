using UnityEngine;
using System.Collections.Generic;

public class DialogueManagerScript : MonoBehaviour {

	public TextAsset dialogueAsset;

	public bool showOnLeft = false;
	public bool showOnRight = false;

	private DialogueSequence seq;
	private DialogueNode current;

	public string text {
		get{
			return current.getText();
		}
	}

	public string speaker {
		get{
			return current.getSpeaker();
		}
	}

	public List<string> options {
		get{
			List<string> nodes, texts;
			current.getOptions(out nodes, out texts);
			return texts;
		}
	}

	// Use this for initialization
	void Start () {
		if(dialogueAsset == null) {
			Debug.LogError("No TextAsset assigned to this script!");
			Application.Quit();
			return;
		}
		seq = new DialogueSequence(dialogueAsset);
		current = seq.getStartingNode();
	}

	public void NewDialogueTree(TextAsset nodes){
		dialogueAsset = nodes;
		Start();
	}

	public void SelectOption(int index) {
		List<string> nodes, texts;
		current.getOptions(out nodes, out texts);
		current = seq.getNode(nodes[index]);
	}

	public void SelectOption(string optionText) {
		List<string> nodes, texts;
		current.getOptions(out nodes, out texts);

		for(int i = 0; i < nodes.Count; i++) {
			if(texts[i] == optionText) {
				current = seq.getNode(nodes[i]);
				break;
			}
		}
	}

	public void OnGUI() {
		if(showOnLeft){
			GUILayout.BeginArea(new Rect(0,0,Screen.width/2.0f,Screen.height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.Label(speaker);
			GUILayout.Label(text);
			for(int i = 0; i < options.Count; i++){
				if(GUILayout.Button(options[i])){
					SelectOption(i);
					if(options.Count == 0){
						showOnLeft = false;
						current = seq.getStartingNode();
					}
				}
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		if(showOnRight){
			GUILayout.BeginArea(new Rect(Screen.width/2.0f,0,Screen.width/2.0f,Screen.height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.Label(speaker);
			GUILayout.Label(text);
			for(int i = 0; i < options.Count; i++){
				if(GUILayout.Button(options[i])){
					SelectOption(i);
					if(options.Count == 0){
						showOnRight = false;
						current = seq.getStartingNode();
					}
				}
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}


}
