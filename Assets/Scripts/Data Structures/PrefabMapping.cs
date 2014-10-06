using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public static class PrefabMapper {

	public static string PrefabLocation = "Assets/Prefabs/MappedObjects/";
	public static string SpriteLocation = "Assets/Sprites/";
	public static string SoundLocation = "Assets/SoundEffects/";
	
	public static Dictionary<string, string> activatorToGateMap = new Dictionary<string, string>()
	{
		{"Switch", "switchgate"},
		{"Switch1", "switchgate1"},
		{"Switch2", "switchgate2"},
		{"Switch3", "switchgate3"},
		{"Switch4", "switchgate4"},
		{"SwitchDefault", "switchgate"},
		{"SwitchDefault1", "switchgate1"},
		{"SwitchDefault2", "switchgate2"},
		{"SwitchDefault3", "switchgate3"},
		{"SwitchDefault4", "switchgate4"},
		{"Button", "buttongate"},
		{"Button1", "buttongate1"},
		{"Button2", "buttongate2"},
		{"Button3", "buttongate3"},
		{"Button4", "buttongate4"}
	};

	public static Dictionary<string, string> originals = new Dictionary<string, string>()
	{
		{"pushblock", "defaultPushblock"},
		{"switch", "defaultSwitch"},
		{"switch1", "defaultSwitch"},
		{"switch2", "defaultSwitch"},
		{"switch3", "defaultSwitch"},
		{"switch4", "defaultSwitch"},
		{"switchdefault1", "defaultSwitch"},
		{"switchdefault2", "defaultSwitch"},
		{"switchdefault3", "defaultSwitch"},
		{"switchdefault4", "defaultSwitch"},
		{"button", "defaultButton"},
		{"button1", "defaultButton"},
		{"button2", "defaultButton"},
		{"button3", "defaultButton"},
		{"button4", "defaultButton"},
		{"portal", "defaultBidirectionalPortal"},
		{"portal1", "defaultBidirectionalPortal"},
		{"portal2", "defaultBidirectionalPortal"},
		{"portal3", "defaultBidirectionalPortal"},
		{"portal4", "defaultBidirectionalPortal"},
		{"sendportal", "defaultSendPortal"},
		{"sendportal1", "defaultSendPortal"},
		{"sendportal2", "defaultSendPortal"},
		{"sendportal3", "defaultSendPortal"},
		{"sendportal4", "defaultSendPortal"},
		{"receiveportal", "defaultReceivePortal"},
		{"receiveportal1", "defaultReceivePortal"},
		{"receiveportal2", "defaultReceivePortal"},
		{"receiveportal3", "defaultReceivePortal"},
		{"receiveportal4", "defaultReceivePortal"},
		{"switchgate", "defaultSwitchGate"},
		{"switchgate1", "defaultSwitchGate"},
		{"switchgate2", "defaultSwitchGate"},
		{"switchgate3", "defaultSwitchGate"},
		{"switchgate4", "defaultSwitchGate"},
		{"buttongate", "defaultButtonGate"},
		{"buttongate1", "defaultButtonGate"},
		{"buttongate2", "defaultButtonGate"},
		{"buttongate3", "defaultButtonGate"},
		{"blocker", "defaultBlocker"},
		{"FenceHBar", "FenceHBar"},
		{"FencePost", "FencePost"},
		{"FenceVBar", "FenceVBar"},
		{"FenceVBarTop", "FenceVBarTop"},
		{"FenceVBarBottom", "FenceVBarBottom"},
		{"pushblockdefault", "defaultPushblock"}
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

	public static Dictionary<string, string> JOURNEY2_LEFT_PREFABS = new Dictionary<string, string>()
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

	public static Dictionary<string, string> JOURNEY2_RIGHT_PREFABS = new Dictionary<string, string>()
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
		{ "buttongate3", "ButtonGate3" },
		{ "pushblock1x2", "Pushblock1x2" },
		{ "pushblock1x3", "Pushblock1x3" },
		{ "pushblock2x1", "Pushblock2x1" },
		{ "pushblock3x1", "Pushblock3x1" }
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
	
	public static Dictionary<string, string> S_PREFABS = new Dictionary<string, string>()
	{
		{ "pushblockdefault", "PushblockDefault" },
		{ "pushblock", "Pushblock" },
		{ "blocker", "Blocker" },
		{ "switch1", "Switch1" },
		{ "switch2", "Switch2" },
		{ "switch3", "Switch3" },
		{ "switchdefault1", "SwitchDefault1" },
		{ "switchdefault2", "SwitchDefault2" },
		{ "switchdefault3", "SwitchDefault3" },
		{ "switchgate1", "SwitchGate1" },
		{ "switchgate2", "SwitchGate2" },
		{ "switchgate3", "SwitchGate3" },
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
		{"J2Left", JOURNEY2_LEFT_PREFABS},
		{"J1Right", JOURNEY1_RIGHT_PREFABS},
		{"J2Right", JOURNEY2_RIGHT_PREFABS},
		{"S", S_PREFABS},
		{"default", originals}
	};
}
