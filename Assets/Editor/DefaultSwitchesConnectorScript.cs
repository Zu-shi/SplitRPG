using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class DefaultSwitchesConnectorScript : MonoBehaviour {
	
	[Tooltip("The first prefab to merge. Autofilled as secondmost recent by importer script for default switches.")]
	GameObject Prefab1;

	[Tooltip("The second prefab to merge. Autofilled as secondmost recent by importer script for default switches.")]
	GameObject Prefab2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
