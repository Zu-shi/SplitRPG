using UnityEngine;
using System.Collections;

public static class Utils {

	public static GameObject Create(GameObject _go, float _x, float _y){
		return (GameObject)(Object.Instantiate (_go, new Vector2 (_x, _y), Quaternion.identity));
	}
	
	//public static 

}
