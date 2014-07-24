using UnityEngine;
using System.Collections;

public static class Utils {

	// Declaration of a simple delegate type
	public delegate void VoidDelegate();


	public static GameObject Create(GameObject _go, float _x, float _y){
		return (GameObject)(Object.Instantiate (_go, new Vector2 (_x, _y), Quaternion.identity));
	}

	/// <summary>
	/// Are the values equal within the tolerance
	/// </summary>
	public static bool CloseValues(float v1, float v2, float tolerance){
		if(Mathf.Abs(v1 - v2) < tolerance)
			return true;
		else 
			return false;
	}

	/// <summary>
	/// Move a value towards a destination value by speed.
	/// </summary>
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

	/// <summary>
	/// Clamp the specified value to between min and max.
	/// </summary>
	public static float Clamp(float value, float min, float max){
		if(value <= min)
			return min;
		else if (value >= max)
			return max;
		else
			return value;
	}

	/// <summary>
	/// Is the layer's name equal to 'name'?
	/// </summary>
	public static bool LayerIs(int layer, string name){
		if(LayerMask.NameToLayer(name) == layer){
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Is the player on the specified tile?
	/// </summary>
	public static bool PlayerIsOnTile(int x, int y, bool leftPlayer){
		PlayerControllerScript player = (leftPlayer ? Globals.playerLeft : Globals.playerRight);
		if(player.tileX == x && player.tileY == y){
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Is the player on the specified tile?
	/// </summary>
	public static bool PlayerIsOnTile(int x, int y, int leftOrRightLayer){
		return PlayerIsOnTile(x, y, LayerIs(leftOrRightLayer, "Left"));
	}

	/// <summary>
	/// Convert pixels to tiles
	/// </summary>
	public static int PixelsToTiles(int val){
		return val / Globals.PIXELS_PER_TILE;
	}

	/// <summary>
	/// Returns a vector that points in the specified direction e.g. Left -> (-1, 0)
	/// </summary>
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
