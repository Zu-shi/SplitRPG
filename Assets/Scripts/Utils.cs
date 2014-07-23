using UnityEngine;
using System.Collections;

public static class Utils {

	// Declaration of a simple delegate type
	public delegate void VoidDelegate();

	static PlayerControllerScript playerLeft = null;
	static PlayerControllerScript playerRight = null;

	static void FindPlayers(){
		if(playerLeft == null){
			playerLeft = GameObject.Find("PlayerLeft").GetComponent<PlayerControllerScript>();
			playerRight = GameObject.Find("PlayerRight").GetComponent<PlayerControllerScript>();
		}
	}


	public static GameObject Create(GameObject _go, float _x, float _y){
		return (GameObject)(Object.Instantiate (_go, new Vector2 (_x, _y), Quaternion.identity));
	}

	public static bool CloseValues(float v1, float v2, float tolerance){
		if(Mathf.Abs(v1 - v2) < tolerance)
			return true;
		else 
			return false;
	}

	public static float MoveValueTowards(float value, float dest, float speed){
		if(Utils.CloseValues(value, dest, speed +.000001f)){
			value = dest;
		} else if (value < dest){
			value += speed;
		} else if (value > dest){
			value -= speed;
		}
		return value;
	}

	public static float Clamp(float value, float min, float max){
		if(value <= min)
			return min;
		else if (value >= max)
			return max;
		else
			return value;
	}

	public static int Round(float value){
		/*
		if (value < Mathf.Floor (value) + 0.5f) {
			return Mathf.FloorToInt (value);
		} else {
			return Mathf.FloorToInt (value);
		}*/
		return Mathf.CeilToInt (value - 0.5f);
	}


	public static bool LayerIsLeft(int layer){
		if(LayerMask.NameToLayer("Left") == layer){
			return true;
		} else {
			return false;
		}
	}

	public static bool PlayerIsOnTile(int x, int y, bool leftPlayer){
		FindPlayers();

		PlayerControllerScript player = (leftPlayer ? playerLeft : playerRight);
		if(player.tileX == x && player.tileY == y){
			return true;
		} else {
			return false;
		}
	}

	public static bool PlayerIsOnTile(int x, int y, int leftOrRightLayer){
		return PlayerIsOnTile(x, y, LayerIsLeft(leftOrRightLayer));
	}

}
