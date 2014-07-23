using UnityEngine;
using System.Collections;

public static class Utils {

	// Declaration of a simple delegate type
	public delegate void VoidDelegate();

	static PlayerControllerScript _playerLeft = null;
	static PlayerControllerScript playerLeft{
		get{
			if(_playerLeft == null)
				_playerLeft = GameObject.Find("PlayerLeft").GetComponent<PlayerControllerScript>();
			return _playerLeft;
		}
	}

	static PlayerControllerScript _playerRight = null;
	static PlayerControllerScript playerRight{
		get{
			if(_playerRight == null)
				_playerRight = GameObject.Find("PlayerRight").GetComponent<PlayerControllerScript>();
			return _playerRight;
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

	public static int PixelsToTiles(int val){
		return val / Globals.PIXELS_PER_TILE;
	}

}
