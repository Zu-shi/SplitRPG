using UnityEngine;
using System.Collections.Generic;

public class LevelManagerScript : _Mono{

	// Only used in editor, copied to _levelPrefabs at start
	public GameObject[] levelPrefabs;

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
		SaveCheckpoint(currentLeftLevelPrefab, currentRightLevelPrefab);
	}

	public void LoadLastCheckpoint() {
		if(savedLeftLevel == null || savedRightLevel == null) {
			LoadLevels(currentLeftLevel, currentRightLevel);
		} else {
			savedLeftLevel.transform.FindChild("SpawnPoint").position = savedLeftPlayerPos;
			savedRightLevel.transform.FindChild("SpawnPoint").position = savedRightPlayerPos;
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

			leftSpawn = left.transform.FindChild("SpawnPoint");
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
			
			rightSpawn = right.transform.FindChild("SpawnPoint");
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

	private void MoveLeftCharacterToSpawnPoint() {
		currentLeftLevelPrefab.SetActive(true);
		Globals.playerLeft.gameObject.SetActive(true);
		GameObject.FindGameObjectWithTag("PlayerLeft").transform.position = leftSpawn.position;
	}

	private void MoveRightCharacterToSpawnPoint() {
		currentRightLevelPrefab.SetActive(true);
		Globals.playerRight.gameObject.SetActive(true);
		GameObject.FindGameObjectWithTag("PlayerRight").transform.position = rightSpawn.position;
		Globals.roomManager.MoveCamerasToPoint( new Vector2(rightSpawn.transform.position.x, rightSpawn.transform.position.y));

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
			Invoke( "_Load" , 0.5f );
		}
		else
			Debug.LogError("Not enough level prefabs assigned!");
	}
}
