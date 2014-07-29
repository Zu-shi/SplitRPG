using UnityEngine;
using System.Collections.Generic;

public class LevelManager : _Mono{

	public GameObject[] levelPrefabs;

	private List<GameObject> _levelPrefabs;

	private GameObject currentLevelPrefab;

	public string currentLevel {
		get {
			if(currentLevelPrefab != null) {
				return currentLevelPrefab.name;
			}
			else
				return null;
		}
	}

	public bool LoadLevel(string name) {
		for(int i = 0; i < _levelPrefabs.Count; i++) {
			if(_levelPrefabs[i].name == name) {
				return LoadLevel(_levelPrefabs[i]);
			}
		}
		return false;
	}

	public bool LoadLevel(GameObject prefab) {
		GameObject go = (GameObject)GameObject.Instantiate(prefab);

		if(go != null) {
			GameObject.DestroyImmediate(currentLevelPrefab);
			currentLevelPrefab = go;
			
			GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
			if(spawnPoint != null) {
				Globals.cameraLeft.FadeTransition(MovePlayersToSpawnPoint, FinishedLoading);
				Globals.cameraRight.FadeTransition(MovePlayersToSpawnPoint, FinishedLoading);
			}
			else {
				Debug.LogError("No spawn point set for level: " + prefab.name);
			}

			return true;
		}
		else
			return false;

	}

	private void MovePlayersToSpawnPoint() {
		GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
		GameObject.FindGameObjectWithTag("PlayerLeft").transform.position = spawnPoint.transform.position;
		GameObject.FindGameObjectWithTag("PlayerRight").transform.position = spawnPoint.transform.position;
		Globals.roomManager.MoveCamerasToPoint( new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y));
	}

	public void FinishedLoading() {
		Debug.Log("LevelManagerCallBack");
	}

	private void _Load() {
		LoadLevel(_levelPrefabs[0]);
	}

	public void Start() {
		_levelPrefabs = new List<GameObject>();
		for(int i = 0; i < levelPrefabs.Length; i++) {
			_levelPrefabs.Add(levelPrefabs[i]);
		}

		if(levelPrefabs.Length > 0) {
			Invoke( "_Load" , 0.5f );
		}
		else
			Debug.LogError("No level prefabs assigned!");
	}
}
