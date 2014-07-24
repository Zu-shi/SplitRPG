using UnityEngine;
using System.Collections;

public static class Utils {

	// Declaration of a simple delegate type
	public delegate void VoidDelegate();


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

	public static bool LayerIsLeft(int layer){
		if(LayerMask.NameToLayer("Left") == layer){
			return true;
		} else {
			return false;
		}
	}

	public static bool PlayerIsOnTile(int x, int y, bool leftPlayer){
		PlayerControllerScript player = (leftPlayer ? Globals.playerLeft : Globals.playerRight);
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

	// Left -> (-1, 0), Up -> (0, 1), etc
	public static Vector2 DirectionToVector(Direction d){
		switch(d){
		case Direction.LEFT:
			return new Vector2(-1, 0);
		case Direction.RIGHT:
			return new Vector2(1, 0);
		case Direction.UP:
			return new Vector2(0, 1);
		case Direction.DOWN:
			return new Vector2(0, -1);
		default:
			return new Vector2(0, 0);
		}
	}

}
