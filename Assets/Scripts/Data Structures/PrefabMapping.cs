using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public static class PrefabMapper {

	public static string PrefabLocation = "Assets/Prefabs/MappedObjects/";
	public static string SpriteLocation = "Assets/Sprites/";
	public static string SoundLocation = "Assets/SoundEffects/";

	public static Dictionary<string, string> originals = new Dictionary<string, string>()
	{
		{"pushblock", "defaultPushblock"},
		{"switch", "defaultSwitch"},
		{"button", "defaultButton"},
		{"button1", "defaultButton"},
		{"button2", "defaultButton"},
		{"button3", "defaultButton"},
		{"button4", "defaultButton"},
		{"switchgate", "defaultSwitchGate"},
		{"switchgate1", "defaultSwitchGate"},
		{"switchgate2", "defaultSwitchGate"},
		{"switchgate3", "defaultSwitchGate"},
		{"switchgate4", "defaultSwitchGate"},
		{"buttongate", "defaultButtonGate"},
		{"buttongate1", "defaultButtonGate"},
		{"buttongate2", "defaultButtonGate"},
		{"buttongate3", "defaultButtonGate"},
		{"blocker", "defaultBlocker"}
	};
	
	public static Dictionary<string, string> JOURNEY1_LEFT_PREFABS = new Dictionary<string, string>()
	{
		{ "pushblock", "Pushblock" },
		{ "blocker", "Blocker" },
		{ "switch1", "Switch1" },
		{ "switch2", "Switch2" },
		{ "switch3", "Switch3" },
		{ "switch4", "Switch4" },
		{ "switchgate1", "SwitchGate1" },
		{ "switchgate2", "SwitchGate2" },
		{ "switchgate3", "SwitchGate3" },
		{ "switchgate4", "SwitchGate4" },
		{ "button1", "Button1" },
		{ "button2", "Button2" },
		{ "button3", "Button3" },
		{ "buttongate1", "ButtonGate1" },
		{ "buttongate2", "ButtonGate2" },
		{ "buttongate3", "ButtonGate3" }
	};
	
	public static Dictionary<string, string> JOURNEY1_RIGHT_PREFABS = new Dictionary<string, string>()
	{
		{ "pushblock", "Pushblock" },
		{ "blocker", "Blocker" },
		{ "switch1", "Switch1" },
		{ "switch2", "Switch2" },
		{ "switch3", "Switch3" },
		{ "switch4", "Switch4" },
		{ "switchgate1", "SwitchGate1" },
		{ "switchgate2", "SwitchGate2" },
		{ "switchgate3", "SwitchGate3" },
		{ "switchgate4", "SwitchGate4" },
		{ "button1", "Button1" },
		{ "button2", "Button2" },
		{ "button3", "Button3" },
		{ "buttongate1", "ButtonGate1" },
		{ "buttongate2", "ButtonGate2" },
		{ "buttongate3", "ButtonGate3" }
	};

	public static Dictionary< string, Dictionary<string, string> > maps = new Dictionary<string, Dictionary<string, string> >()
	{
		{"J1Left", JOURNEY1_LEFT_PREFABS},
		{"J1Right", JOURNEY1_RIGHT_PREFABS},
		{"default", originals}
	};
}
