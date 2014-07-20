using System.Collections.Generic;
using UnityEngine;

public class DialogNode
{

	private struct option
	{
		public string nodeName;
		public string optionText;
	}

	private string name;
	private string speaker;
	private string text;

	private List<option> options;
	
	public DialogNode(string name, string speaker, string text)
	{
		this.name = name;
		this.speaker = speaker;
		this.text = text;
		this.options = new List<option>();
	}

	public void addOption(string nodeName, string text)
	{
		option tmp = new option();
		tmp.nodeName = nodeName;
		tmp.optionText = text;
		this.options.Add(tmp);
	}

	public string getSpeaker() { return this.speaker; }
	public string getName() { return this.name; }
	public string getText() { return this.text; }
	public int getNumOptions() { return options.Count; }

	public void setSpeaker(string speaker) { this.speaker = speaker; }
	public void setText(string text) { this.text = text; }

	public void getOptions(out List<string> nodeNames, out List<string> optionTexts)
	{
		nodeNames = new List<string>(this.options.Count);
		optionTexts = new List<string>(this.options.Count);


		for(int i = 0; i < options.Count; i++)
		{
			nodeNames.Insert(i, options[i].nodeName);
			optionTexts.Insert(i, options[i].optionText);
		}
	}

	public void removeOption(string name)
	{
		for(int i = 0; i < options.Count; i++)
		{
			if(options[i].nodeName.Equals(name))
				options.RemoveAt(i);
		}
	}




}