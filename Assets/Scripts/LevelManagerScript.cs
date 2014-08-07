using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Level loading and saving.
/// </summary>
/// <cauthor>Tyler Wallace</authorc>
public class LevelManagerScript : _Mono{

	// Only used in editor, copied to _levelPrefabs at start
	public GameObject[] levelPrefabs;
	private List<GameObject> _levelPrefabs;

	public bool loadOnStart;

	[Tooltip("You must assign these if you do not want to load on start.")]
	public GameObject preloadedLeftLevel, preloadedRightLevel;
	
	private GameObject currentLeftLevelPrefab, currentRightLevelPrefab;
	private GameObject savedLeftLevel, savedRightLevel;

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

	private Transform leftSpawn {
		get {
			return Utils.FindChildRecursive(currentLeftLevelPrefab, "Starting").GetChild(0);
		}
	}
	
	private Transform rightSpawn {
		get {
			return Utils.FindChildRecursive(currentRightLevelPrefab, "Starting").GetChild(0);
		}
	}

	public bool SaveCheckpoint(GameObject leftLevel, GameObject rightLevel) {
		if(leftLevel == null || rightLevel == null) {
			Debug.LogError("Attempted to save null level.");
			return false;
		}
		Destroy(savedLeftLevel);
		Destroy(savedRightLevel);

		savedLeftLevel = (GameObject)Instantiate(leftLevel);
		savedLeftLevel.name = leftLevel.name;
		Transform lsp = Utils.FindChildRecursive(savedLeftLevel, "Starting").GetChild(0);
		lsp.position = Globals.playerLeft.transform.position;
		lsp.GetComponent<BoxCollider2D>().center = Vector3.zero;
		PlayerPrefs.SetFloat("LeftX", lsp.position.x);
		PlayerPrefs.SetFloat("LeftY", lsp.position.y);
		savedLeftLevel.SetActive(false);

		savedRightLevel = (GameObject)Instantiate(rightLevel);
		savedRightLevel.name = rightLevel.name;
		Transform rsp = Utils.FindChildRecursive(savedRightLevel, "Starting").GetChild(0);
		rsp.position = Globals.playerRight.transform.position;
		rsp.GetComponent<BoxCollider2D>().center = Vector3.zero;
		PlayerPrefs.SetFloat("RightX", rsp.position.x);
		PlayerPrefs.SetFloat("RightY", rsp.position.y);
		savedRightLevel.SetActive(false);

		PlayerPrefs.SetString("LeftLevel", currentLeftLevel);
		PlayerPrefs.SetString("RightLevel", currentRightLevel);

		return true;
	}

	public void SaveCheckpoint() {
		SaveCheckpoint(currentLeftLevelPrefab, currentRightLevelPrefab);
	}

	public void LoadLastCheckpoint() {
		if(savedLeftLevel == null || savedRightLevel == null) {
			LoadLevels(currentLeftLevel, currentRightLevel);
		} else {
			LoadLevels(savedLeftLevel, savedRightLevel);
		}
	}

	public void ReloadCurrentLevels() {
		LoadLevels(currentLeftLevel, currentRightLevel);
	}

	public void LoadMainMenu() {
		Application.LoadLevel(0);
	}
	
	public void LoadSerializedGame() {
		LoadLevels(PlayerPrefs.GetString("LeftLevel"), PlayerPrefs.GetString("RightLevel"));

		// Current levels are set in the above call to LoadLevels
		leftSpawn.position = new Vector3(PlayerPrefs.GetFloat("LeftX"), PlayerPrefs.GetFloat("LeftY"), leftSpawn.position.z);
		rightSpawn.position = new Vector3(PlayerPrefs.GetFloat("RightX"), PlayerPrefs.GetFloat("RightY"), rightSpawn.position.z);
		leftSpawn.GetComponent<BoxCollider2D>().center = Vector3.zero;
		rightSpawn.GetComponent<BoxCollider2D>().center = Vector3.zero;
	}

	public bool LoadLevels(string leftLevelName, string rightLevelName) {
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
			return LoadLevels(left, right);
		}
		else{
			Debug.Log("Failed to load levels: " + leftLevelName + ", " + rightLevelName);
			return false;
		}
	}

	public bool LoadLevels(GameObject leftLevel, GameObject rightLevel) {
		if(leftLevel == null || rightLevel == null) {
			Debug.LogError("Cannont load null level.");
			return false;
		}

		GameObject left = (GameObject)GameObject.Instantiate(leftLevel, Vector3.zero, Quaternion.identity);
		GameObject right = (GameObject)GameObject.Instantiate(rightLevel, Vector3.zero, Quaternion.identity);
		left.SetActive(false);
		right.SetActive(false);
		Globals.playerLeft.gameObject.SetActive(false);
		Globals.playerRight.gameObject.SetActive(false);

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
	
		if(rightSpawn != null) {
			Globals.cameraRight.FadeTransition( (Utils.VoidDelegate)MoveRightCharacterToSpawnPoint + (Utils.VoidDelegate)FinishMoving, null);
		}
		else {
			Debug.LogError("No spawn point set for level: " + right.name);
		}

		return true;
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
		Globals.roomManager.MoveCamerasToPoint( new Vector2(rightSpawn.position.x, rightSpawn.position.y));
		if(PlayerPrefs.GetString("LoadGame") == "true") {
			PlayerPrefs.SetString("LoadGame", "false");
			SaveCheckpoint();
		}
	}

	private void _Load() {
		if(PlayerPrefs.GetString("LoadGame") == "true") {
			LoadSerializedGame();
		}
		else {
			LoadLevels(_levelPrefabs[0], _levelPrefabs[1]);
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
}
