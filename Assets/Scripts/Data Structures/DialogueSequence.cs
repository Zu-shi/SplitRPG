using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence
{
	public int NumberOfNodes
	{
		get { return nodes.Count; }
	}

	private List<DialogueNode> nodes;

	public DialogueSequence()
	{
		this.nodes = new List<DialogueNode>();
	}

	public DialogueSequence(TextAsset nodeFile) : this()
	{
		System.IO.StringReader sr = new System.IO.StringReader(nodeFile.text);

		while(sr.Peek() != -1 && sr.ReadLine().Trim().Equals("NODE:"))  // Read each node in.
		{
			string name = sr.ReadLine().Trim().Substring(6);
			string speaker = sr.ReadLine().Trim().Substring(9);
			string text = sr.ReadLine().Trim().Substring(6);
			string tmp;
			while( (tmp = sr.ReadLine().Trim()) != "ENDTEXT")
			{
				text += tmp + " ";
			}
			DialogueNode newNode = new DialogueNode(name, speaker, text);
			while( (tmp = sr.ReadLine().Trim()) == "OPTION:" )
			{
				name = sr.ReadLine().Trim();
				text = sr.ReadLine().Trim();
				newNode.addOption(name, text);
			}
			this.addNode(newNode);

		}
	}

	public void addNode(DialogueNode node)
	{
		nodes.Add(node);
	}

	public void addNode(string name, string speaker, string text, string[] optNames, string[] optTexts)
	{
		DialogueNode tmp = new DialogueNode(name, speaker, text);
		if (optNames != null && optTexts != null)
		{
			for(int i = 0; i < optNames.Length; i++)
			{
				tmp.addOption(optNames[i], optTexts[i]);
			}
		}
		this.addNode(tmp);
	}

	public void addNode(string name, string speaker, string text)
	{
		this.addNode(name, speaker, text, null, null);
	}

	private DialogueNode findNode(string name)
	{
		for(int i = 0; i < this.nodes.Count; i++)
		{
			if(this.nodes[i].getName().Equals(name))
				return this.nodes[i];
		}
		Debug.LogError("Could not find node: " + name);

		return null;
	}

	public void addOptionToNode(string nodeName, string optName, string optText)
	{
		DialogueNode tmp = this.findNode(nodeName);
		if (tmp == null) 
		{
			Debug.LogError("In DialogSequence.addOptionToNode: Could not find node " + nodeName);
			ListNodes();
			return;
		}
		else
		{
			tmp.addOption(optName, optText);
		}
	}

	public void removeNode(string name)
	{
		for(int i = 0; i < nodes.Count; i++) 
		{
			if(nodes[i].getName().Equals(name))
			{
				nodes.RemoveAt(i);
				continue;
			}
			nodes[i].removeOption(name);
		}

	}

	public DialogueNode getNode(string name)
	{
		return this.findNode(name);
	}

	public DialogueNode getStartingNode()
	{
		DialogueNode tmp = this.findNode("Start");
		if (tmp == null)
			return nodes[0];
		else
			return tmp;
	}

	public void ListNodes()
	{
		Debug.Log("Known nodes: ");
		for(int i = 0; i < nodes.Count; i++)
			Debug.Log(nodes[i].getName());
	}
	
}