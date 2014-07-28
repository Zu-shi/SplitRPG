using UnityEngine;
using System.Collections.Generic;

public class LevelManager : _Mono{

	public GameObject[] levelPrefabs;

	private static List<GameObject> _levelPrefabs;

	private static GameObject currentLevelPrefab;

	public static string currentLevel {
		get {
			if(currentLevelPrefab != null) {
				return currentLevelPrefab.name;
			}
			else
				return null;
		}
	}

	public static bool LoadLevel(string name) {
		for(int i = 0; i < _levelPrefabs.Count; i++) {
			if(_levelPrefabs[i].name == name) {
				return LoadLevel(_levelPrefabs[i]);
			}
		}
		return false;
	}

	public static bool LoadLevel(GameObject prefab) {
		GameObject go = (GameObject)GameObject.Instantiate(prefab);

		if(go != null) {
			GameObject.DestroyImmediate(currentLevelPrefab);
			currentLevelPrefab = go;
			return true;
		}
		else
			return false;

	}

	public void Start() {
		_levelPrefabs = new List<GameObject>();
		for(int i = 0; i < levelPrefabs.Length; i++) {
			_levelPrefabs.Add(levelPrefabs[i]);
		}

		if(levelPrefabs.Length > 0) {
			LoadLevel(_levelPrefabs[0]);
		}
		else
			Debug.LogError("No level prefabs assigned!");
	}
}
