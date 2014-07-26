using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data carrier class for a simple branching dialogue system.
/// A node carries information about who is currently speaking and what they are saying.
/// </summary>
public class DialogueNode {

	/// <summary>
	/// The name of this node.
	/// </summary>
	public readonly string name;

	/// <summary>
	/// The person or entity that is speaking in this node.
	/// </summary>
	public string speaker;

	/// <summary>
	/// The main contents of this node. This represents the piece of dialogue that the speaker
	/// is delivering in this node.
	/// </summary>
	public string text;

	/// <summary>
	/// Initializes a new instance of the <see cref="DialogueNode"/> class.
	/// </summary>
	/// <param name="name">The name of this DialogueNode.</param>
	/// <param name="speaker">The person or entity speaking in this DialogueNode.</param>
	/// <param name="text">The piece of dialogue that the speaker is dilivering in this DialogueNode.</param>
	public DialogueNode(string name, string speaker, string text) {
		this.name = name;
		this.speaker = speaker;
		this.text = text;
	}
		
}