using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class PrefabImporterScript : MonoBehaviour {
	
	public bool convertSpritesToPrefabs = false;
	public bool J1Left;
	public bool J1Right;
	public bool J2Left;
	public bool J2Right;

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
			
			if (J2Left) {
				ImportPrefabsFromIamges("J2Left");
			}

			if (J2Right) {
				ImportPrefabsFromIamges("J2Right");	
				ImportHeavyBlocks();
			}
		} 

	}

	private void ImportHeavyBlocks(){
		string map = "J2Right";
		SetUpHeavyBlock( map, "2x1", new Vector2[]{new Vector2(0, 0), new Vector2(2, 0)} );
		SetUpHeavyBlock( map, "3x1", new Vector2[]{new Vector2(0, 0), new Vector2(2, 0), new Vector2(4, 0)} );
		SetUpHeavyBlock( map, "1x2", new Vector2[]{new Vector2(0, 0), new Vector2(0, -2)} );
		SetUpHeavyBlock( map, "1x3", new Vector2[]{new Vector2(0, 0), new Vector2(0, -2), new Vector2(0, -4)} );

	}

	private void SetUpHeavyBlock(string map, string dimensions, Vector2[] body){
		GameObject go;
		SpriteRenderer sr;
		string objname;

		objname = "Pushblock";
		go  = getOriginalPrefabOfObject (objname.ToLower());
		sr = go.GetComponent<SpriteRenderer> ();

		objname += dimensions;
		sr.sprite = retrieveSpriteByName (map, objname);

		PushBlockColliderScript ps = go.GetComponent<PushBlockColliderScript> ();
		ps.pushSound = AssetDatabase.LoadAssetAtPath(PrefabMapper.SoundLocation + map + "Push.wav", typeof(AudioClip)) as AudioClip;
		MovementScript ms = go.GetComponent<MovementScript> ();
		ms.body = (Vector2[])body.Clone ();

		foreach (Vector2 bodypart in body) {
			if(bodypart != Vector2.zero){
				BoxCollider2D bc = ms.gameObject.AddComponent<BoxCollider2D> ();
				bc.size = new Vector2(2, 2);
				bc.center = new Vector2(bodypart.x, bodypart.y);
			}
		}

		SaveAndDestory (map, objname, go);
	}

	private void ImportPrefabsFromIamges(string map){
		GameObject go;
		SpriteRenderer sr;
		string objname;
		string[] children;

		objname = "Pushblock";
		go  = getOriginalPrefabOfObject (objname.ToLower());
		sr = go.GetComponent<SpriteRenderer> ();
		sr.sprite = retrieveSpriteByName (map, objname);
		go.GetComponent<PushBlockColliderScript> ().pushSound = AssetDatabase.LoadAssetAtPath(PrefabMapper.SoundLocation + map + "Push.wav", typeof(AudioClip)) as AudioClip;
		SaveAndDestory (map, objname, go);

		objname = "Blocker";
		go  = getOriginalPrefabOfObject (objname.ToLower());
		sr = go.GetComponent<SpriteRenderer> ();
		sr.sprite = retrieveSpriteByName (map, objname);
		SaveAndDestory (map, objname, go);
		
		for (int i = 1; i <= 4; i ++) {
			objname = "Switch";
			go = getOriginalPrefabOfObject (objname.ToLower ());
			SwitchScript ss = go.GetComponent<SwitchScript> ();
			ss.onSprite = retrieveSpriteByName (map, objname + "On");
			ss.offSprite = retrieveSpriteByName (map, objname + "Off");
			sr = go.GetComponent<SpriteRenderer> ();
			sr.sprite = ss.onSprite;
			SaveAndDestory (map, objname + i.ToString(), go);
		}

		for (int i = 1; i <= 3; i ++) {
			objname = "Button" + i.ToString();
			go = getOriginalPrefabOfObject (objname.ToLower ());
			ButtonScript bs = go.GetComponent<ButtonScript> ();
			bs.onSprite = retrieveSpriteByName (map, objname + "On");
			bs.offSprite = retrieveSpriteByName (map, objname + "Off");
			sr = go.GetComponent<SpriteRenderer> ();
			sr.sprite = bs.onSprite;
			SaveAndDestory (map, objname, go);
		}

		for (int i = 1; i <= 4; i ++) {
			if(map!="J1Left"  && map != "J1Right"){
				objname = "Portal" + i.ToString();
				go = getOriginalPrefabOfObject (objname.ToLower ());
				PortalSenderScript bs = go.GetComponent<PortalSenderScript> ();
				//bs.onSprite = retrieveSpriteByName (map, objname + "On");
				//bs.offSprite = retrieveSpriteByName (map, objname + "Off");
				//sr = go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer> ();
				sr = go.GetComponent<SpriteRenderer> ();
				sr.sprite = retrieveSpriteByName (map, objname);
				SaveAndDestory (map, objname, go);
			}
		}

		for (int i = 1; i <= 3; i ++) {
			objname = "ButtonGate" + i.ToString();
			go = getOriginalPrefabOfObject (objname.ToLower ());
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
			go = getOriginalPrefabOfObject (objname.ToLower());
			GateScript gs = go.GetComponent<GateScript> ();
			gs.closedSpriteH = retrieveSpriteByName (map, objname + "ClosedH");
			gs.closedSpriteV = retrieveSpriteByName (map, objname + "ClosedV");
			gs.openSprite = retrieveSpriteByName (map, objname + "Open");
			
			sr = go.GetComponent<SpriteRenderer> ();
			sr.sprite = gs.closedSpriteH;
			SaveAndDestory (map, objname, go);
		}
		
		objname = "FenceHBar";
		go  = getOriginalPrefabOfObject (objname);
		go.GetComponent<SpriteRenderer> ().sprite = retrieveSpriteByName (map, objname);
		SaveAndDestory (map, objname, go);
		
		objname = "FenceVBar";
		go  = getOriginalPrefabOfObject (objname);
		go.GetComponent<SpriteRenderer> ().sprite = retrieveSpriteByName (map, objname);
		SaveAndDestory (map, objname, go);
		
		objname = "FenceVBarBottom";
		go  = getOriginalPrefabOfObject (objname);
		go.GetComponent<SpriteRenderer> ().sprite = retrieveSpriteByName (map, objname);
		SaveAndDestory (map, objname, go);
		
		objname = "FenceVBarTop";
		go  = getOriginalPrefabOfObject (objname);
		go.GetComponent<SpriteRenderer> ().sprite = retrieveSpriteByName (map, objname);
		SaveAndDestory (map, objname, go);
		
		objname = "FencePost";
		go  = getOriginalPrefabOfObject (objname);
		go.GetComponent<SpriteRenderer> ().sprite = retrieveSpriteByName (map, objname);
		SaveAndDestory (map, objname, go);
	}


	private GameObject getOriginalPrefabOfObject(string name){
		Debug.Log (name);
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