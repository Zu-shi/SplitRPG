using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Level loading and saving.
/// </summary>
/// <cauthor>Tyler Wallace</authorc>
public class LevelManagerScript : _Mono{

	// Only used in editor, copied to _levelPrefabs at start
	public GameObject[] levelPrefabs;
	public bool loadOnStart;

	[Tooltip("You must assign these if you do not want to load on start.")]
	public GameObject preloadedLeftLevel, preloadedRightLevel;

	private List<GameObject> _levelPrefabs;

	private GameObject currentLeftLevelPrefab, currentRightLevelPrefab;
	private GameObject savedLeftLevel, savedRightLevel;
	private Vector3 savedLeftPlayerPos, savedRightPlayerPos;

	private Transform leftSpawn, rightSpawn;

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

	public bool SaveCheckpoint(GameObject leftLevel, GameObject rightLevel) {
		if(leftLevel == null || rightLevel == null) {
			Debug.LogError("Attempted to load null level.");
			return false;
		}
		savedLeftPlayerPos = Globals.playerLeft.transform.position;
		savedRightPlayerPos = Globals.playerRight.transform.position;
		
		Destroy(savedLeftLevel);
		Destroy(savedRightLevel);

		savedLeftLevel = (GameObject)Instantiate(leftLevel);
		savedLeftLevel.name = leftLevel.name;
		savedLeftLevel.SetActive(false);

		savedRightLevel = (GameObject)Instantiate(rightLevel);
		savedRightLevel.name = rightLevel.name;
		savedRightLevel.SetActive(false);

		return true;
	}

	public void SaveCheckpoint() {
		Debug.Log("Saving Checkpoint.");
		SaveCheckpoint(currentLeftLevelPrefab, currentRightLevelPrefab);
	}

	public void LoadLastCheckpoint() {
		Debug.Log("Loading Checkpoint.");
		if(savedLeftLevel == null || savedRightLevel == null) {
			LoadLevels(currentLeftLevel, currentRightLevel);
		} else {
			savedLeftLevel.transform.FindChild("Starting").GetChild(0).position = savedLeftPlayerPos;
			savedRightLevel.transform.FindChild("Starting").GetChild(0).position = savedRightPlayerPos;
			LoadLevels(savedLeftLevel, savedRightLevel);
		}
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
		GameObject left = (GameObject)GameObject.Instantiate(leftLevel, Vector3.zero, Quaternion.identity);
		GameObject right = (GameObject)GameObject.Instantiate(rightLevel, Vector3.zero, Quaternion.identity);
		left.SetActive(false);
		right.SetActive(false);
		Globals.playerLeft.gameObject.SetActive(false);
		Globals.playerRight.gameObject.SetActive(false);

		Globals.roomManager.enabled = false;
		// Left level
		if(left != null) {
			left.name = leftLevel.name;
			GameObject.DestroyImmediate(currentLeftLevelPrefab);
			currentLeftLevelPrefab = left;

			leftSpawn = left.transform.FindChild("Starting").GetChild(0);
			if(leftSpawn != null) {
				Globals.cameraLeft.FadeTransition(MoveLeftCharacterToSpawnPoint, FinishedLoading);
			}
			else {
				Debug.LogError("No spawn point set for level: " + left.name);
			}
		}
		else{
			Debug.LogError("Failed to instantiate level prefab");
			Globals.roomManager.enabled = true;
			return false;
		}

		// Right level
		if(right != null) {
			right.name = rightLevel.name;
			GameObject.DestroyImmediate(currentRightLevelPrefab);
			currentRightLevelPrefab = right;
			
			rightSpawn = right.transform.FindChild("Starting").GetChild(0);
			if(rightSpawn != null) {
				Globals.cameraRight.FadeTransition(MoveRightCharacterToSpawnPoint, FinishedLoading);
			}
			else {
				Debug.LogError("No spawn point set for level: " + right.name);
			}
		}
		else{
			Debug.LogError("Failed to instantiate level prefab");
			Globals.roomManager.enabled = true;
			return false;
		}

		_currentLeftLevel = leftLevel.name;
		_currentRightLevel = rightLevel.name;

		return true;

	}

	public void LoadMainMenu() {
		Application.LoadLevel(0);
	}

	private void MoveLeftCharacterToSpawnPoint() {
		currentLeftLevelPrefab.SetActive(true);
		Globals.playerLeft.gameObject.SetActive(true);
		GameObject.FindGameObjectWithTag("PlayerLeft").transform.position = leftSpawn.TransformPoint(leftSpawn.gameObject.GetComponent<BoxCollider2D>().center);
	}

	private void MoveRightCharacterToSpawnPoint() {
		currentRightLevelPrefab.SetActive(true);
		Globals.playerRight.gameObject.SetActive(true);
		GameObject.FindGameObjectWithTag("PlayerRight").transform.position = rightSpawn.TransformPoint(rightSpawn.gameObject.GetComponent<BoxCollider2D>().center);
		Globals.roomManager.MoveCamerasToPoint( new Vector2(rightSpawn.transform.position.x, rightSpawn.transform.position.y));

	}

	public void SaveGame() {
		// TODO JSON serialization?
	}

	private void FinishedLoading() {
		Globals.roomManager.enabled = true;
	}

	public void ReloadCurrentLevels() {
		LoadLevels(currentLeftLevel, currentRightLevel);
	}

	private void _Load() {
		LoadLevels(_levelPrefabs[0], _levelPrefabs[1]);
	}

	public void Start() {
		_levelPrefabs = new List<GameObject>();
		for(int i = 0; i < levelPrefabs.Length; i++) {
			_levelPrefabs.Add(levelPrefabs[i]);
		}

		if(levelPrefabs.Length > 1) {
			if(loadOnStart) {
				Invoke( "_Load" , 0.5f );
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
