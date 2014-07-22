using UnityEngine;
using System.Collections.Generic;

public class DialogueManagerScript : MonoBehaviour {

	public TextAsset dialogueAsset;

	public bool showOnLeft = false;
	public bool showOnRight = false;
	public float textSpeed = 5.0f;

	private DialogueSequence seq;
	private DialogueNode current;
	private string displayText = "";
	private bool scrolling = false;
	private float scrollingTimer = 0;
	private bool fading = false;

	public string nodeName {
		get{
			return current.getName();
		}
	}

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

	public void Update() {
		if( (showOnLeft != showOnRight) && !fading ) { // We have been told to become visible on one side, but we are not fading a side out.
			fading = true;

			// Disable movement
			GameObject.Find("PlayerLeft").GetComponent<PlayerControllerScript>().enabled = false;
			GameObject.Find("PlayerRight").GetComponent<PlayerControllerScript>().enabled = false;
			GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputScript>().enabled = false;

			if(showOnLeft)
				GameObject.Find("GameManager").GetComponent<GameManagerScript>().FadeDownRightSide();
			else
				GameObject.Find("GameManager").GetComponent<GameManagerScript>().FadeDownLeftSide();

		}

		if( (showOnLeft == showOnRight) && fading ) {
			fading = false;
			if(!showOnLeft){
				// Enable Movement
				// TODO Make a centralized way of doing this
				GameObject.Find("PlayerLeft").GetComponent<PlayerControllerScript>().enabled = true;
				GameObject.Find("PlayerRight").GetComponent<PlayerControllerScript>().enabled = true;
				GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputScript>().enabled = true;
			}
			GameObject.Find("GameManager").GetComponent<GameManagerScript>().FadeUpRightSide();
			GameObject.Find("GameManager").GetComponent<GameManagerScript>().FadeUpLeftSide();
		}

		
		if(!scrolling && (showOnLeft || showOnRight)) { // We are showing on at least one side but not scrolling.
			scrolling = true;
		}
		if(scrolling && !(showOnLeft || showOnRight)) { // We are not showing on either side, but we are scrolling.
			scrolling = false;
		}

		if(scrolling) {
			scrollingTimer += Time.deltaTime;
			if( displayText != current.getText() ){
				if(scrollingTimer >= 1.0f/textSpeed){
					scrollingTimer = 0;
					displayText += current.getText()[displayText.Length];
				}

			} else {
				scrolling = false;
			}
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
		displayText = "";
		scrolling = true;
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
		displayText = "";
		scrolling = true;
	}

	public void OnGUI() {
		//TODO centralize this to avoid potential multiple dialogues bugs
		if(showOnLeft){
			GUILayout.BeginArea(new Rect(0,0,Screen.width/2.0f,Screen.height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(GUILayout.Width(300));
			GUILayout.FlexibleSpace();
			GUILayout.Label(speaker);
			GUILayout.Label(displayText, GUILayout.Height(150));
			for(int i = 0; i < options.Count; i++){
				if(scrolling){
					GUILayout.Space(20);
				} else {
					if(GUILayout.Button(options[i], GUILayout.Height(20))){
						SelectOption(i);
						if(options.Count == 0){
							showOnLeft = false;
							current = seq.getStartingNode();
						}
					}
				}
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		if(showOnRight){
			GUILayout.BeginArea(new Rect(Screen.width/2.0f,0,Screen.width/2.0f,Screen.height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(GUILayout.Width(300));
			GUILayout.FlexibleSpace();
			GUILayout.Label(speaker);
			GUILayout.Label(displayText, GUILayout.Height(150));
			for(int i = 0; i < options.Count; i++){
				if(scrolling){
					GUILayout.Space(20);
				} else {
					if(GUILayout.Button(options[i], GUILayout.Height(20))){
						SelectOption(i);
						if(options.Count == 0){
							showOnLeft = false;
							current = seq.getStartingNode();
						}
					}
				}
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}


}
