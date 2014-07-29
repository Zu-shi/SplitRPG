using UnityEngine;
using System.Collections.Generic;

public class LevelManager : _Mono{

	// Only used in editor, copied to _levelPrefabs at start
	public GameObject[] levelPrefabs;

	private List<GameObject> _levelPrefabs;

	private GameObject currentLeftLevelPrefab, currentRightLevelPrefab;
	private Transform leftSpawn, rightSpawn;

	public string currentLeftLevel {
		get {
			if(currentLeftLevelPrefab != null) {
				return currentLeftLevelPrefab.name;
			}
			else
				return null;
		}
	}

	public string currentRightLevel {
		get {
			if(currentRightLevelPrefab != null) {
				return currentRightLevelPrefab.name;
			}
			else
				return null;
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
			return false;
		}
	}

	public bool LoadLevels(GameObject leftLevel, GameObject RightLevel) {
		GameObject left = (GameObject)GameObject.Instantiate(leftLevel, Vector3.zero, Quaternion.identity);
		GameObject right = (GameObject)GameObject.Instantiate(RightLevel, Vector3.zero, Quaternion.identity);
		left.SetActive(false);
		right.SetActive(false);
		Globals.playerLeft.gameObject.SetActive(false);
		Globals.playerRight.gameObject.SetActive(false);

		Globals.roomManager.enabled = false;
		// Left level
		if(left != null) {
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

	public void FinishedLoading() {
		Debug.Log("LevelManagerCallBack");
		Globals.roomManager.enabled = true;
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
