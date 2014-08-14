using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class PrefabImporterScript : _Mono {

	public bool convertSpritesToPrefabs = false;
	public bool J1Left;
	public bool J1Right;

	void Start(){
		convertSpritesToPrefabs = false;
	}
	
	void Update(){
		
		// When a person checks the check box, update the z value
		if(!Application.isPlaying){
			if(!convertSpritesToPrefabs){
				return;
			} else {
				convertSpritesToPrefabs = false;
			}
			if (J1Left) {
				ImportPrefabsFromIamges("J1Left");	
			}

			if (J1Right) {
				ImportPrefabsFromIamges("J1Right");	
			}
		} 

	}

	private void ImportPrefabsFromIamges(string map){
		GameObject go;
		SpriteRenderer sr;
		string objname;

		objname = "Pushblock";
		go  = getOriginalPrefabOfObject (map, objname.ToLower());
		sr = go.GetComponent<SpriteRenderer> ();
		sr.sprite = retrieveSpriteByName (map, objname);
		SaveAndDestory (map, objname, go);
		
		objname = "Blocker";
		go  = getOriginalPrefabOfObject (map, objname.ToLower());
		sr = go.GetComponent<SpriteRenderer> ();
		sr.sprite = retrieveSpriteByName (map, objname);
		SaveAndDestory (map, objname, go);
		
		for (int i = 1; i <= 4; i ++) {
			objname = "Switch";
			go = getOriginalPrefabOfObject (map, objname.ToLower ());
			SwitchScript ss = go.GetComponent<SwitchScript> ();
			ss.onSprite = retrieveSpriteByName (map, objname + "On");
			ss.offSprite = retrieveSpriteByName (map, objname + "Off");
			sr = go.GetComponent<SpriteRenderer> ();
			sr.sprite = ss.onSprite;
			SaveAndDestory (map, objname + i.ToString(), go);
		}

		for (int i = 1; i <= 3; i ++) {
			objname = "Button" + i.ToString();
			go = getOriginalPrefabOfObject (map, objname.ToLower ());
			ButtonScript bs = go.GetComponent<ButtonScript> ();
			bs.onSprite = retrieveSpriteByName (map, objname + "On");
			bs.offSprite = retrieveSpriteByName (map, objname + "Off");
			sr = go.GetComponent<SpriteRenderer> ();
			sr.sprite = bs.onSprite;
			SaveAndDestory (map, objname, go);
		}

		for (int i = 1; i <= 3; i ++) {
			objname = "ButtonGate" + i.ToString();
			go = getOriginalPrefabOfObject (map, objname.ToLower ());
			GateScript gs = go.GetComponent<GateScript> ();
			gs.closedSpriteH = retrieveSpriteByName (map, objname + "ClosedH");
			gs.closedSpriteV = retrieveSpriteByName (map, objname + "ClosedV");
			gs.openSprite = retrieveSpriteByName (map, objname + "Open");

			sr = go.GetComponent<SpriteRenderer> ();
			sr.sprite = gs.closedSpriteH;
			SaveAndDestory (map, objname, go);
		}
		
		for (int i = 1; i <= 4; i ++) {
			objname = "SwitchGate" + i.ToString();
			go = getOriginalPrefabOfObject (map, objname.ToLower());
			GateScript gs = go.GetComponent<GateScript> ();
			gs.closedSpriteH = retrieveSpriteByName (map, objname + "ClosedH");
			gs.closedSpriteV = retrieveSpriteByName (map, objname + "ClosedV");
			gs.openSprite = retrieveSpriteByName (map, objname + "Open");
			
			sr = go.GetComponent<SpriteRenderer> ();
			sr.sprite = gs.closedSpriteH;
			SaveAndDestory (map, objname, go);
		}
		/*
		for (int i = 1; i < 5; i ++) {
			objname = "Button";
			go = getOriginalPrefabOfObject (map, objname.ToLower ());
			ButtonScript bs = go.GetComponent<ButtonScript> ();
			bs.onSprite = retrieveSpriteByName (map, objname + "On" + i.ToString);
			bs.offSprite = retrieveSpriteByName (map, objname + "Off" + i.ToString);
			SaveAndDestory (map, objname + i.ToString, go);
		}*/
	}

	private GameObject getOriginalPrefabOfObject(string map, string name){
		string goLocation = PrefabMapper.originals [name];
		GameObject goPrefab = AssetDatabase.LoadAssetAtPath(PrefabMapper.PrefabLocation + goLocation + ".prefab", typeof(GameObject)) as GameObject;
		Utils.assert (goPrefab != null);
		GameObject go = GameObject.Instantiate(goPrefab) as GameObject;
		return go;
	}

	private Sprite retrieveSpriteByName(string map, string name){
		Sprite sp = AssetDatabase.LoadAssetAtPath (PrefabMapper.SpriteLocation + map + "/" + name + ".png", typeof(Sprite)) as Sprite;
		Debug.Log (PrefabMapper.SpriteLocation + map + "/" + name + ".png");
		Utils.assert (sp != null);
		return sp;
	}

	private void SaveAndDestory(string map, string name, GameObject go){
		PrefabUtility.CreatePrefab(PrefabMapper.PrefabLocation + map + "/" + name + ".prefab", go);
		DestroyImmediate (go);
	}
}