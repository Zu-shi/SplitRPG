using UnityEngine;
using System.Collections.Generic;

public class DialogueManagerScript : MonoBehaviour {

	[Tooltip("The text file that contains the dialogue node information for this Game Object.")]
	public TextAsset dialogueAsset;

	[Tooltip("If set, this dialogue will show up on the left side. If showOnRight is not also set, then the right side will be dimmed.")]
	public bool showOnLeft = false;
	
	[Tooltip("If set, this dialogue will show up on the right side. If showOnLeft is not also set, then the left side will be dimmed.")]
	public bool showOnRight = false;

	[Tooltip("The speed, in characters per second, that text will be written to the screen.")]
	public float textSpeed = 5.0f;

	/// <summary>
	/// The DialogueSequence that contains the data for the current conversation.
	/// </summary>
	private DialogueSequence seq;

	/// <summary>
	/// The name of the current node.
	/// </summary>
	private string current;

	/// <summary>
	/// The text that is currently being displayed for the current node.
	/// This is updated as more text scrolls onto the screen.
	/// </summary>
	private string displayText = "";

	/// <summary>
	/// True if we are scrolling text 'typewriter-style' onto the screen.
	/// </summary>
	private bool scrolling = false;

	/// <summary>
	/// How long until we update the display text.
	/// </summary>
	private float scrollingTimer = 0;

	/// <summary>
	/// True if one side of the screen is fading out.
	/// </summary>
	private bool fading = false;

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

				scrolling = false;
			}
			GameObject.Find("GameManager").GetComponent<GameManagerScript>().FadeUpRightSide();
			GameObject.Find("GameManager").GetComponent<GameManagerScript>().FadeUpLeftSide();
		}

		if( (showOnLeft || showOnRight) && !scrolling && (displayText != text) ) {
			scrolling = true;
		}

		if(scrolling) {
			scrollingTimer += Time.deltaTime;

			while(scrollingTimer >= 1.0f/textSpeed) {
				if( displayText != this.text ) {
					scrollingTimer -= 1.0f/textSpeed;
					displayText += this.text[displayText.Length];
				} else {
					scrolling = false;
					scrollingTimer = 0;
					break;
				} 
			}
		}
	}

	/// <summary>
	/// Gets the text of the current node.
	/// </summary>
	/// <value>The text.</value>
	private string text {
		get{
			return seq.GetText(current);
		}
	}

	/// <summary>
	/// Gets the speaker of the current node.
	/// </summary>
	/// <value>The speaker.</value>
	private string speaker {
		get{
			return seq.GetSpeaker(current);
		}
	}

	/// <summary>
	/// Gets the options for the current node.
	/// </summary>
	/// <value>The options.</value>
	private List<DialogueSequence.Option> options {
		get{
			return seq.GetOptions(current);
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
		current = seq.startingNodeName;
	}

	/// <summary>
	/// Load new a new dialogue sequence from the given asset.
	/// </summary>
	/// <param name="nodes">Nodes.</param>
	public void NewDialogueTree(TextAsset nodes){
		dialogueAsset = nodes;
		Start();
	}

	/// <summary>
	/// Selects the dialogue option at the specified index in the list of current options.
	/// </summary>
	/// <param name="index">Index.</param>
	public void SelectOption(int index) {
		current = options[index].to;
		displayText = "";
		scrolling = true;
	}

	/// <summary>
	/// Selects the option with the given description from the list of current options.
	/// If more than one option has the same description, this is probably a mistake and 
	/// the first option is the only one returned.
	/// </summary>
	/// <param name="description">Description.</param>
	public void SelectOption(string description) {
		List<DialogueSequence.Option> opts = options;
		for(int i = 0; i < opts.Count; i++) {
			if(opts[i].description == description) {
				current = opts[i].to;
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
					if(GUILayout.Button(options[i].description, GUILayout.Height(20))){
						SelectOption(i);
						if(options.Count == 0){
							showOnLeft = false;
							current = seq.startingNodeName;
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
					if(GUILayout.Button(options[i].description, GUILayout.Height(20))){
						SelectOption(i);
						if(options.Count == 0){
							showOnRight = false;
							current = seq.startingNodeName;
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
