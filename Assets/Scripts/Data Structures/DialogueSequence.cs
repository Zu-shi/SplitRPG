using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection class that represents a single dialogue tree and its branches. This class uses an internal representation
/// consisting of a list of options and a list of DialogueNodes. Clients are intended to use the string interfaces that this
/// class provides for read-only operations.
/// </summary>
public class DialogueSequence {
	
	public struct Option {
		public string from;
		public string to;
		public string description;

		public static bool operator ==(Option left, Option right) {
			return (left.from == right.from) && (left.to == right.to) && (left.description == right.description);
		}

		public static bool operator !=(Option left, Option right) {
			return !(left == right);
		}
	}

	/// <summary>
	/// The list of all dialogue nodes that this sequence is tracking.
	/// </summary>
	private List<DialogueNode> nodes;

	/// <summary>
	/// All of the branching options.
	/// </summary>
	private List<Option> options;

	/// <summary>
	/// The total number of nodes in this sequence.
	/// </summary>
	public int numberOfNodes {
		get{
			return nodes.Count;
		}
	}

	/// <summary>
	/// Gets the name of the starting node.
	/// </summary>
	/// <value>The name of the starting node.</value>
	public string startingNodeName {
		get{
			if( FindNode("Start") == null) {
				if(nodes.Count > 0){
					return nodes[0].name;
				}
				else {
					return null;
				}
			}
			else {
				return "Start";
			}
		}
	}

	public DialogueSequence() {
		this.nodes = new List<DialogueNode>();
		this.options = new List<Option>();
	}

	public DialogueSequence(TextAsset nodeFile) : this() {
		System.IO.StringReader sr = new System.IO.StringReader(nodeFile.text);

		while(sr.Peek() != -1 && sr.ReadLine().Trim().Equals("NODE:")) {  // Read each node in.
			string name = sr.ReadLine().Trim().Substring(6);
			string speaker = sr.ReadLine().Trim().Substring(9);
			string text = sr.ReadLine().Trim().Substring(6);
			string tmp;

			while( (tmp = sr.ReadLine().Trim()) != "ENDTEXT") {
				text += tmp + " ";
			}

			DialogueNode newNode = new DialogueNode(name, speaker, text);

			while( (tmp = sr.ReadLine().Trim()) == "OPTION:" ) {
				name = sr.ReadLine().Trim();
				text = sr.ReadLine().Trim();
				options.Add( new Option{ from = newNode.name, to = name, description = text } );
			}
			this.AddNode(newNode);
		}

		//TODO Verify that all options are valid.
	}

	/// <summary>
	/// Gets the text that is contained in the node with the given name.
	/// </summary>
	/// <returns>Node's text..</returns>
	/// <param name="node">The name of the node.</param>
	public string GetText(string node) {
		DialogueNode tmp = FindNode(node);
		if(tmp != null) {
			return tmp.text;
		}
		else {
			return null;
		}
	}

	/// <summary>
	/// Gets the speaker of the specified node.
	/// </summary>
	/// <returns>The speaker.</returns>
	/// <param name="node">The name of the node to seach for.</param>
	public string GetSpeaker(string node) {
		DialogueNode tmp = FindNode(node);
		if(tmp != null) {
			return tmp.speaker;
		}
		else {
			return null;
		}
	}

	/// <summary>
	/// Gets the node with the given name. This method should only be used
	/// when the returned node needs to be modified; the string based methods of this
	/// class are the intended interface for read-only operations.
	/// </summary>
	/// <returns>The node.</returns>
	/// <param name="name">The name of the node to search for.</param>
	public DialogueNode GetNode(string name) {
		return FindNode(name);
	}

	/// <summary>
	/// Adds a new node to this dialogue sequence.
	/// </summary>
	/// <returns><c>true</c>, if node was added, <c>false</c> otherwise.</returns>
	/// <param name="node">The node to add.</param>
	public bool AddNode(DialogueNode node) {
		nodes.Add(node);
		return true;
	}

	/// <summary>
	/// Adds a new node with the given fields to this dialogue sequence.
	/// </summary>
	/// <returns><c>true</c>, if node was added, <c>false</c> otherwise.</returns>
	/// <param name="name">Name of the new node.</param>
	/// <param name="speaker">Speaker of the new node.</param>
	/// <param name="text">Text of the new node.</param>
	public bool AddNode(string name, string speaker, string text) {
		return AddNode( new DialogueNode(name, speaker, text) );
	}

	/// <summary>
	/// Searches the sequence for the node with the given name and returns it.
	/// </summary>
	/// <returns>The node, null if not found.</returns>
	/// <param name="name">Name of the node to search for.</param>
	private DialogueNode FindNode(string name) {
		for(int i = 0; i < this.nodes.Count; i++) {
			if(this.nodes[i].name == name)
				return this.nodes[i];
		}

		return null;
	}

	/// <summary>
	/// Add a new branching option to this dialogue sequence. The new option will branch from 'from' to 'to'
	/// and will have the given description.
	/// </summary>
	/// <returns><c>true</c>, if option was added, <c>false</c> otherwise.</returns>
	/// <param name="fromNode">From node.</param>
	/// <param name="toNode">To node.</param>
	/// <param name="desc">Description.</param>
	public bool AddOption(string fromNode, string toNode, string desc) {
		if( FindNode(fromNode) == null || FindNode(toNode) == null) {
			return false;
		}
		else {
			options.Add( new Option { from = fromNode, to = toNode, description = desc } );
			return true;
		}
	}

	/// <summary>
	/// Remove the specified option from this dialogue sequence.
	/// </summary>
	/// <returns><c>true</c>, if option was removed, <c>false</c> otherwise.</returns>
	/// <param name="opt">The option to remove.</param>
	public bool RemoveOption(Option opt) {
		for(int i = 0; i < options.Count; i++) {
			if(options[i] == opt) {
				options.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Remove the specified node from this dialogue sequence.
	/// </summary>
	/// <returns><c>true</c>, if node was removed, <c>false</c> otherwise.</returns>
	/// <param name="name">The name of the node to remove.</param>
	public bool RemoveNode(string name) {
		bool rv = false;
		for(int i = 0; i < nodes.Count; i++) {
			if(nodes[i].name == name) {
				nodes.RemoveAt(i);
				rv = true;
				break;
			}
		}

		if(rv) {
			for(int i = 0; i < options.Count; i++) {
				if(options[i].to == name || options[i].from == name) {
					options.RemoveAt(i);
				}
			}
		}

		return rv;
	}

	/// <summary>
	/// Print the names of all of the nodes in this dialogue sequence to the console.
	/// </summary>
	public void ListNodes() {
		Debug.Log("Known nodes: ");
		for(int i = 0; i < nodes.Count; i++)
			Debug.Log(nodes[i].name);
	}

	/// <summary>
	/// Gets all of the options the branch away from the node with the given name;
	/// that is, all of the returned options will have 'node' in their 'from' field.
	/// </summary>
	/// <returns>The options.</returns>
	/// <param name="name">The name of the node to get options for.</param>
	public List<DialogueSequence.Option> GetOptions(string name) {
		List<Option> opts = new List<Option>();

		for(int i = 0; i < options.Count; i++) {
			if(options[i].from == name) {
				opts.Add(options[i]);
			}
		}

		return opts;
	}


}