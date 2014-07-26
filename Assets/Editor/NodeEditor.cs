using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NodeEditor : EditorWindow {
	
	private List<Rect> windows = new List<Rect>();
	private DialogueSequence seq = new DialogueSequence();
	private List<string> nodeNames = new List<string>();

	private bool createNode = false;
	private bool createOpt = false;
	private string newNodeName, fromNode, toNode, optText;
	private string export = "Export To";
	private Vector2 scrollPos = new Vector2(0,0);

	[MenuItem("Window/Node editor")]
	static void ShowEditor() {
		NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
	}

	public void OnGUI() {

		if(seq.numberOfNodes != nodeNames.Count) // Everything is broken, panic
		{
			seq = new DialogueSequence();
			nodeNames = new List<string>();
			windows = new List<Rect>();
		}
		
		BeginWindows();
		if(createOpt) // Creating a new connection between nodes
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			// Labels
			GUILayout.Label("From Node");
			GUILayout.Label("To Node");
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			// Text fileds
			fromNode = GUILayout.TextField(fromNode, GUILayout.MinWidth(200), GUILayout.MaxWidth(200));
			toNode = GUILayout.TextField(toNode, GUILayout.MinWidth(200), GUILayout.MaxWidth(200));
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Text");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			optText = GUILayout.TextArea(optText, GUILayout.MinWidth(300), GUILayout.MinHeight(50));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Create"))
			{
				seq.AddOption(fromNode, toNode, optText);
				fromNode = toNode = optText = null;
				createOpt = false;
			}
			if(GUILayout.Button("Cancel"))
			{
				fromNode = toNode = optText = null;
				createOpt = false;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		else if(createNode) // Creating a new node
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Node Name: ", GUILayout.ExpandWidth(false));
			newNodeName = GUILayout.TextField(newNodeName, GUILayout.Width(250));
			GUILayout.EndHorizontal();
			GUILayout.Space(50);
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Create"))
			{
				if(!nodeNames.Contains(newNodeName))
				{
					seq.AddNode(newNodeName, "Speaker", "Text");
					windows.Add( new Rect(30,30, 200, 150));
					nodeNames.Add(newNodeName);
				}

				newNodeName = null;
				createNode = false;

			}
			if(GUILayout.Button("Cancel"))
			{
				newNodeName = null;
				createNode = false;
			}

			GUILayout.EndHorizontal();

			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		else // Viewing the tree
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Create Node")) {
				newNodeName = "New Node";
				createNode = true;
			}
			if(GUILayout.Button("Add Option"))
			{
				createOpt = true;
				fromNode = "From";
				toNode = "To";
				optText = "Text";
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();


			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			export = GUILayout.TextField(export, GUILayout.Width(200));
			if(GUILayout.Button("Export to File"))
				ExportToFile(export);

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			DrawAllLines();
			
			for(int i = 0; i < nodeNames.Count; i++)
			{
				windows[i] = GUILayout.Window(i, windows[i], DrawNodeWindow, nodeNames[i]);
			}

		}
		EndWindows();
	}

	private void ExportToFile(string fileName)
	{
		Debug.Log("Exporting nodes to: " + fileName);
		if(System.IO.File.Exists(Application.dataPath + "/" + fileName))
			System.IO.File.Delete(Application.dataPath + "/" +fileName);
		List<string> exportedNodes = new List<string>();
		for(int i = 0; i < nodeNames.Count; i++)
		{
			PrintNodeToFile(nodeNames[i], fileName);
		}
	}

	private void PrintNodeToFile(string node, string file)
	{
		string tmp = "NODE:\n";
		tmp += "\tNAME: " + node + "\n";
		tmp += "\tSPEAKER: " + seq.GetSpeaker(node) + "\n";
		tmp += "\tTEXT: " + seq.GetText(node) + "\n";
		tmp += "\tENDTEXT\n";

		List<DialogueSequence.Option> opts = seq.GetOptions(node);

		for(int i = 0; i < opts.Count; i++)
		{
			tmp += "\tOPTION:\n";
			tmp += "\t\t" + opts[i].to + "\n";
			tmp += "\t\t" + opts[i].description + "\n";
		}

		tmp += "\tENDOPTS\n";

		System.IO.File.AppendAllText(Application.dataPath + "/" +file, tmp);
	}

	private void DrawAllLines()
	{
		for(int i = 0; i < nodeNames.Count; i++)
		{
			List<DialogueSequence.Option> opts = seq.GetOptions(nodeNames[i]);

			Rect sourceRect = windows[i];

			for(int j = 0; j < opts.Count; j++)
			{
				int index = FindIndexOf(opts[j].to);
				if(index >= 0 && index < nodeNames.Count)
				{
					Rect targetRect = windows[index];
					DrawNodeCurve(sourceRect, targetRect);
				}
				else
				{
					Debug.Log("Index: " + index);
					Debug.Log("Name: " + opts[j].to);
				}
			}
		}
	}

	private void DrawNodeWindow(int id) {

		DialogueNode tmp = seq.GetNode(nodeNames[id]);
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Speaker", GUILayout.ExpandWidth(false));
		tmp.speaker = GUILayout.TextField(tmp.speaker);
		GUILayout.EndHorizontal();

		GUILayout.Label("Text");

		tmp.text = GUILayout.TextArea(tmp.text, GUILayout.MinHeight(40));

		List<DialogueSequence.Option> opts = seq.GetOptions(tmp.name);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Options", GUILayout.ExpandWidth(false));
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Add Option"))
		{
			fromNode = nodeNames[id];
			toNode = "To";
			optText = "Text";
			createOpt = true;
		}
		GUILayout.EndHorizontal();

		for(int i = 0; i < opts.Count; i++)
		{
			GUILayout.BeginHorizontal();

			GUILayout.BeginVertical(GUILayout.ExpandWidth(false));
			GUILayout.Label("Connecting To");
			GUILayout.Label("Text");
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(opts[i].to);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(opts[i].description);
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();

			if(GUILayout.Button("Delete", GUILayout.ExpandHeight(true)))
			{
				seq.RemoveOption(opts[i]);
			}

			GUILayout.EndHorizontal();

		}

		GUILayout.FlexibleSpace();

		if(GUILayout.Button("Delete this Node"))
		{
			seq.RemoveNode(nodeNames[id]);
			windows.RemoveAt(id);
			nodeNames.RemoveAt(id);
		}

		GUILayout.EndVertical();

		GUI.DragWindow();

	}

	private int FindIndexOf(string node)
	{
		for(int i = 0; i < nodeNames.Count; i++)
		{
			if(nodeNames[i].Equals(node))
				return i;
		}
		return -1;
	}

	private void DrawNodeCurve(Rect start, Rect end) {
		Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
		Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
		Vector3 startTan = startPos + Vector3.right * 50;
		Vector3 endTan = endPos + Vector3.left * 50;
		Color shadowCol = new Color(0, 0, 0, 0.06f);
		
		for (int i = 0; i < 3; i++) {// Draw a shadow
			Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
		}
		
		Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 2);
	}
}

