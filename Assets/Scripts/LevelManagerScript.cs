using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Level loading and saving.
/// </summary>
/// <cauthor>Tyler Wallace</authorc>
public class LevelManagerScript : _Mono{

	// Only used in editor, copied to _levelPrefabs at start
	public AudioClip level1Theme;
	public AudioClip level2Theme;
	public AudioClip level3Theme;
	public AudioClip level4Theme;
	public AudioClip cutsceneTheme1;
	public AudioClip cutsceneTheme2;
	public string debugLocation;
	public int startingLevel = 1;

	public GameObject[] levelPrefabs;
	private List<GameObject> _levelPrefabs;
	private List<GameObject> _cachedPersistentObjects = new List<GameObject>();

	public bool loadOnStart;

	[Tooltip("You must assign these if you do not want to load on start.")]
	public GameObject preloadedLeftLevel, preloadedRightLevel;
	
	private GameObject currentLeftLevelPrefab, currentRightLevelPrefab;

	private string _currentLeftLevel;
	public string currentLeftLevel {
		get {
			return _currentLeftLevel;
		}
	}

	private string _currentRightLevel;
	public string currentRightLevel {
		get {
			return _currentRightLevel;
		}
	}

	public Transform leftSpawn {
		get {
			if(debugLocation.Trim() == ""){
				return Utils.FindChildRecursive(currentLeftLevelPrefab, "Starting").GetChild(0);
			}else{
				Transform t;
				//Debug.Log(Utils.FindChildRecursive(currentLeftLevelPrefab, debugLocation));
				if( (t = Utils.FindChildRecursive(currentLeftLevelPrefab, debugLocation) ) != null ){
					return t;
				}else{
					Debug.LogWarning("Cannot find debug location " + debugLocation + ". Loading default location.");
					return Utils.FindChildRecursive(currentLeftLevelPrefab, "Starting").GetChild(0);
				}
			}
		}
	}
	
	public Transform rightSpawn {
		get {
			if(debugLocation.Trim() == ""){
				return Utils.FindChildRecursive(currentRightLevelPrefab, "Starting").GetChild(0);
			}else{
				Transform t;
				//Debug.Log(Utils.FindChildRecursive(currentRightLevelPrefab, debugLocation));
				if( (t = Utils.FindChildRecursive(currentRightLevelPrefab, debugLocation) ) != null ){
					return t;
				}else{
					Debug.LogWarning("Cannot find debug location " + debugLocation + ". Loading default location.");
					return Utils.FindChildRecursive(currentRightLevelPrefab, "Starting").GetChild(0);
				}
			}
		}
	}

	public bool EnableLevels(bool enabled = true) {
		currentLeftLevelPrefab.SetActive(enabled);
		currentRightLevelPrefab.SetActive(enabled);
		return enabled;
	}

	public bool SaveCheckpoint() {
		//Debug.Log("Saving checkpoint...");
		PlayerPrefs.SetFloat("LeftX", Globals.playerLeft.tileX);
		PlayerPrefs.SetFloat("LeftY", Globals.playerLeft.tileY);
		PlayerPrefs.SetFloat("RightX", Globals.playerRight.tileX);
		PlayerPrefs.SetFloat("RightY", Globals.playerRight.tileY);

		PlayerPrefs.SetString("LeftLevel", currentLeftLevel);
		PlayerPrefs.SetString("RightLevel", currentRightLevel);
		
		/*Debug.Log ("LeftLevel = " + PlayerPrefs.GetString("LeftLevel"));
		Debug.Log ("RightLevel = " + PlayerPrefs.GetString("RightLevel"));
		Debug.Log ("LeftX, LeftY = " + PlayerPrefs.GetFloat("LeftX") + ", " + PlayerPrefs.GetFloat("LeftY"));
		Debug.Log ("RightX, RightY = " + PlayerPrefs.GetFloat("RightX") + ", " + PlayerPrefs.GetFloat("RightY"));*/

		return true;
	}

	public void LoadLastCheckpoint() {
		Debug.Log("Loading checkpoint...");
		LoadSerializedGame();
	}

	public void ReloadCurrentLevels() {
		LoadLevels(currentLeftLevel, currentRightLevel);
	}

	public void LoadMainMenu() {
		Application.LoadLevel(0);
	}
	
	public void LoadSerializedGame(bool reloadPersistent = false) {
		/*Debug.Log("Loading serialized game...");
		Debug.Log ("LeftLevel = " + PlayerPrefs.GetString("LeftLevel"));
		Debug.Log ("RightLevel = " + PlayerPrefs.GetString("RightLevel"));
		Debug.Log ("LeftX, LeftY = " + PlayerPrefs.GetFloat("LeftX") + ", " + PlayerPrefs.GetFloat("LeftY"));
		Debug.Log ("RightX, RightY = " + PlayerPrefs.GetFloat("RightX") + ", " + PlayerPrefs.GetFloat("RightY"));*/
		LoadLevels(PlayerPrefs.GetString("LeftLevel"), PlayerPrefs.GetString("RightLevel"), reloadPersistent);

		// LoadLevel callbacks will move the players to these points
		leftSpawn.position = new Vector3(PlayerPrefs.GetFloat("LeftX"), PlayerPrefs.GetFloat("LeftY"), 0);
		rightSpawn.position = new Vector3(PlayerPrefs.GetFloat("RightX"), PlayerPrefs.GetFloat("RightY"), 0);
		leftSpawn.GetComponent<BoxCollider2D>().center = Vector2.zero;
		rightSpawn.GetComponent<BoxCollider2D>().center = Vector2.zero;
	}
	
	private void PlayLevelTheme(GameObject leftLevel, GameObject rightLevel, bool reloadPersistent = false)
	{
		//Check and see if music plays on load.
		//Debug.LogError("LevelManager PlayLevelTheme()");
		if(leftLevel == levelPrefabs[0] && rightLevel == levelPrefabs[1]){
			if(!reloadPersistent){Globals.soundManager.LoadAndPlayClip(cutsceneTheme1);}
			Globals.soundManager.levelMusicClip = level1Theme;
			Debug.Log ("Playing level 1 theme");
		}
		else if(leftLevel == levelPrefabs[2] && rightLevel == levelPrefabs[3]){
			if(!reloadPersistent){Globals.soundManager.LoadAndPlayClip(cutsceneTheme1);}
			Globals.soundManager.levelMusicClip = level2Theme;
			Debug.Log ("Playing level 2 theme");
		}
		else if(leftLevel == levelPrefabs[4] && rightLevel == levelPrefabs[5]){
			if(!reloadPersistent){Globals.soundManager.LoadAndPlayClip(cutsceneTheme1);}
			Globals.soundManager.levelMusicClip = level3Theme;
			Debug.Log ("Playing level 3 theme");
		}
		else if(leftLevel == levelPrefabs[6] && rightLevel == levelPrefabs[7]){
			if(!reloadPersistent){Globals.soundManager.LoadAndPlayClip(cutsceneTheme1);}
			Globals.soundManager.levelMusicClip = level4Theme;
			Debug.Log ("Playing level 4 theme");
		}

		
	} 

	public bool LoadLevels(string leftLevelName, string rightLevelName, bool reloadPersistent = false) {
		GameObject left = null;
		GameObject right = null;
		for(int i = 0; i < _levelPrefabs.Count; i++) {
			if(_levelPrefabs[i].name == leftLevelName) {
				left = _levelPrefabs[i];
			}
			if(_levelPrefabs[i].name == rightLevelName) {
				right = _levelPrefabs[i];
			}
		}
		if(left != null && right != null) {
			return LoadLevels(left, right, reloadPersistent);
		}
		else{
			Debug.LogError("Failed to load levels: " + leftLevelName + ", " + rightLevelName);
			return false;
		}
	}

	public bool LoadLevels(GameObject leftLevel, GameObject rightLevel, bool reloadPersistent = false) {

		PlayLevelTheme(leftLevel, rightLevel, reloadPersistent);

		//Debug.Log("Loading levels...");
		if (leftLevel == null || rightLevel == null) {
			Debug.LogError("Cannont load null level.");
			return false;
		}

		if(reloadPersistent) {
			_cachedPersistentObjects.AddRange(GameObject.FindGameObjectsWithTag("Persistent"));
			foreach(GameObject go in _cachedPersistentObjects) {
				//Debug.Log("Caching: " + go.name);
				go.transform.parent = null;
				go.tag = null;
			}
		}
		
		GameObject left = (GameObject)GameObject.Instantiate(leftLevel, Vector3.zero, Quaternion.identity);
		GameObject right = (GameObject)GameObject.Instantiate(rightLevel, Vector3.zero, Quaternion.identity);

		if ( Utils.FindChildRecursive(left, "Pushblocks(Default)") &&
		    Utils.FindChildRecursive(right, "Pushblocks(Default)") ) {
			LinkPushblocks(left, right);
		} 
		if ( Utils.FindChildRecursive(left, "Switches and Gates(Default)") &&
		    Utils.FindChildRecursive(right, "Switches and Gates(Default)") ){
			LinkSwitches(left, right);
		}

		if(right.GetComponent<LevelPropertiesScript>() != null) {
			right.GetComponent<LevelPropertiesScript>().Start();
			right.GetComponent<LevelPropertiesScript>().Sync();
		}
		else
			Debug.LogWarning("No LevelPropertiesScript found on level: " + right.name);

		left.SetActive(false);
		right.SetActive(false);

		Globals.playerLeft.gameObject.SetActive(false);
		Globals.playerRight.gameObject.SetActive(false);
		Globals.roomManager.enabled = false;

		// Left level
		left.name = leftLevel.name;
		GameObject.DestroyImmediate(currentLeftLevelPrefab);
		currentLeftLevelPrefab = left;
		_currentLeftLevel = left.name;

		if(leftSpawn != null) {
			Globals.cameraLeft.FadeTransition(MoveLeftCharacterToSpawnPoint, null);
		}
		else {
			Debug.LogError("No spawn point set for level: " + left.name);
		}

		// Right level
		right.name = rightLevel.name;
		GameObject.DestroyImmediate(currentRightLevelPrefab);
		currentRightLevelPrefab = right;
		_currentRightLevel = right.name;

		Utils.VoidDelegate mainCallback = (Utils.VoidDelegate)FinishMoving;
		if(reloadPersistent) {
			mainCallback += (Utils.VoidDelegate)FixCachedObjects;
		}
	
		if(rightSpawn != null) {
			Globals.cameraRight.FadeTransition( (Utils.VoidDelegate)MoveRightCharacterToSpawnPoint + mainCallback, FinishLoad);
		}
		else {
			Debug.LogError("No spawn point set for level: " + right.name);
		}

		return true;
	}


	private void FinishLoad() {
		Globals.roomManager.enabled = true;
	}

	private void FixCachedObjects() {
		GameObject[] uncachedObjects = GameObject.FindGameObjectsWithTag("Persistent");

		//Debug.LogError(uncachedObjects.Length + "YAYAYAY");
		foreach(GameObject uncached in uncachedObjects) {
			string name = uncached.name;
			uncached.name = "___uncached___" + name;
			GameObject tmp = FindCachedGameObject(name);
			_cachedPersistentObjects.Remove(tmp);

			tmp.transform.parent = uncached.transform.parent;
			tmp.tag = "Persistent";

			//Update references from gates to old switches to references to new switches
			if(tmp.GetComponent<SwitchScript>() != null){
				tmp.GetComponent<SwitchScript>().InheritGatesFromSwitch(uncached.GetComponent<SwitchScript>());
			}

			Destroy(uncached);
		}
	}

	private GameObject FindCachedGameObject(string name) {
		for(int i = 0; i < _cachedPersistentObjects.Count; i++) {
			if(_cachedPersistentObjects[i].name == name)
				return _cachedPersistentObjects[i];
		}
		Debug.LogError("No cached object named: " + name);
		return null;
	}

	// Used as a callback
	private void MoveLeftCharacterToSpawnPoint() {
		currentLeftLevelPrefab.SetActive(true);
		Globals.playerLeft.gameObject.SetActive(true);
		GameObject.FindGameObjectWithTag("PlayerLeft").transform.position = leftSpawn.TransformPoint(leftSpawn.gameObject.GetComponent<BoxCollider2D>().center);
	}

	// Used as a callback
	private void MoveRightCharacterToSpawnPoint() {
		currentRightLevelPrefab.SetActive(true);
		Globals.playerRight.gameObject.SetActive(true);
		GameObject.FindGameObjectWithTag("PlayerRight").transform.position = rightSpawn.TransformPoint(rightSpawn.gameObject.GetComponent<BoxCollider2D>().center);
	}

	// Helper function for callback
	private void FinishMoving() {
		//Debug.Log("Finishing movement...");
		Globals.roomManager.MoveCamerasToPoint( new Vector2(rightSpawn.position.x, rightSpawn.position.y));
		SaveCheckpoint();
	}

	private void _Load() {
		if(PlayerPrefs.GetString("LoadGame") == "true") {
			LoadSerializedGame();
		}
		else {
			//LoadLevels(_levelPrefabs[0], _levelPrefabs[1]);
			LoadLevels(_levelPrefabs[(startingLevel - 1) * 2], _levelPrefabs[(startingLevel - 1) * 2 + 1]);
		}
	}

	public void Start() {
		_levelPrefabs = new List<GameObject>();
		for(int i = 0; i < levelPrefabs.Length; i++) {
			_levelPrefabs.Add(levelPrefabs[i]);
		}

		if(levelPrefabs.Length > 1) {
			if(loadOnStart) {
				Invoke( "_Load" , 0.1f );
			}
			else {
				if(preloadedLeftLevel == null || preloadedRightLevel == null) {
					// Checkpoint system fails if levelmanager doesn't get access to the game objects.
					Debug.LogError("Preloaded levels not assigned in editor!");
				}
				else {
					currentLeftLevelPrefab = preloadedLeftLevel;
					currentRightLevelPrefab = preloadedRightLevel;
					_currentLeftLevel = currentLeftLevelPrefab.name;
					_currentRightLevel = currentRightLevelPrefab.name;
				}

			}
		}
		else
			Debug.LogError("Not enough level prefabs assigned!");
	}

	void LinkPushblocks(GameObject prefab1, GameObject prefab2){
		//Connect pushblocks by deleting the second copy.
		List<Vector2> toDeleteAtPosition = new List<Vector2> ();
		Transform pushblocksLayerTransform = Utils.FindChildRecursive(prefab1, "Pushblocks(Default)");
		Transform pushblocksLayerTransform2 = Utils.FindChildRecursive(prefab2, "Pushblocks(Default)");
		foreach (Transform pb in pushblocksLayerTransform) {
			PushBlockColliderScript pbs = pb.gameObject.GetComponent<PushBlockColliderScript>();
			Utils.assert(pbs != null, "Check for BlockColliderScript in the Pushblocks(Default) layer for object " + pb.gameObject.name);
			//Debug.Log ("Found pushblock " + pb.name);
			bool foundLink = false;
			
			foreach (Transform pb2 in pushblocksLayerTransform2) {
				PushBlockColliderScript pbs2 = pb2.gameObject.GetComponent<PushBlockColliderScript>();
				Utils.assert(pbs2 != null, "Check for BlockColliderScript in the Pushblocks(Default) layer for object " + pb2.gameObject.name);
				if(pb2.position.Equals(pb.position)){
					//Debug.Log ("Found counterpart " + pb.name);
					foundLink = true;
					pb.gameObject.layer = LayerMask.NameToLayer("Default");
					toDeleteAtPosition.Add(new Vector2(pbs2.x, pbs2.y));
				}
			}
			if(!foundLink){Debug.Log ("Cannot find counterpart for " + prefab1.name + " pushblock at " + pbs.x + ", " + pbs.y + ".");}
		}
		
		foreach (Vector2 pos in toDeleteAtPosition) {
			GameObject toDestroy = null;
			foreach (Transform pb2 in pushblocksLayerTransform2) {
				PushBlockColliderScript pbs2 = pb2.gameObject.GetComponent<PushBlockColliderScript>();
				if(pbs2.x == pos.x && pbs2.y == pos.y){
					toDestroy = pbs2.gameObject;
					break;
				}
			}
			GameObject.DestroyImmediate (toDestroy, true);
		}
	}
	
	//This function looks ugly, clean it later.
	void LinkSwitches(GameObject prefab1,GameObject prefab2){
		//Connect switches by sharing the toggler to watch.
		Transform switchesLayerTransform = Utils.FindChildRecursive(prefab1, "Switches and Gates(Default)");
		Transform switchesLayerTransform2 = Utils.FindChildRecursive(prefab2, "Switches and Gates(Default)");
		foreach (Transform s in switchesLayerTransform) {
			SwitchScript ss;
			if((ss = s.gameObject.GetComponent<SwitchScript>()) != null){
				//Debug.Log ("Found switch " + s.name);
				foreach (Transform s2 in switchesLayerTransform2) {
					//Debug.Log(s.name + " " + s2.name);
					SwitchScript ss2;
					if(s2.name == s.name){
						if((ss2 = s2.gameObject.GetComponent<SwitchScript>()) != null){
							//Debug.Log ("Found counterpart " + s.name);
							//Debug.Log ("Check not actuall the SAME " + (ss2 == ss));
							ss2.twin = ss;
							ss.twin = ss2;
							ss2._toggler = ss._toggler;
							//Debug.Log (ss2._toggler == ss._toggler);
						}else{
							Debug.LogWarning ("Switch that shares a name does not have a switch script.");
						}
					}
				}
			}
		}
	}
}
