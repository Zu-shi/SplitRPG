using UnityEngine;
using System.Collections.Generic;

public class ImporterObjectPairs : MonoBehaviour {

	public static Dictionary<string, GameObject> mappings = new Dictionary<string, GameObject>(); 
	public GameObject pushBlocksPrefab;

	// Use this for initialization
	void Start () {
		mappings.Add ("pushBlocks", pushBlocksPrefab);
	}
}
