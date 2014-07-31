using UnityEngine;
using System.Collections;

/// <summary>
/// Numerical description of a room with some methods for ease of use.
/// </summary>
/// <author>Mark Gardner</author>
public class Room {

	// Rect that defines the room (measured in tiles)
	public Rect roomRect;
	
	/// <summary>
	/// The center of the current room
	/// </summary>
	public Vector2 roomCenter{
		get{ return roomRect.center; }
	}
	/// <summary>
	/// The dimensions of the current room in terms of how many tiles fit inside the room
	/// </summary>
	public Vector2 roomTileDimensions{
		get{ return roomRect.size; }
	}
	
	/// <summary>
	/// The top-most tile that is inside the room.
	/// </summary>
	public int roomTop{
		get{ return Mathf.RoundToInt(roomRect.yMax); }
	}
	/// <summary>
	/// The bottom-most tile that is inside the room.
	/// </summary>
	public int roomBotTile{
		get{ return Mathf.RoundToInt(roomRect.yMin + 1); }
	}
	
	/// <summary>
	/// Bot bound of the room (one unit down from the bottom tile)
	/// </summary>
	public int roomBot{
		get{ return Mathf.RoundToInt(roomRect.yMin); }
	}
	
	/// <summary>
	/// The left-most tile that is inside the room.
	/// </summary>
	public int roomLeft{
		get{ return Mathf.RoundToInt(roomRect.xMin); }
	}
	/// <summary>
	/// The right-most tile that is inside the room.
	/// </summary>
	public int roomRightTile{
		get{ return Mathf.RoundToInt(roomRect.xMax - 1); }
	}
	
	/// <summary>
	/// Right bound of the room (one unit right of the far right tile)
	/// </summary>
	public int roomRight{
		get{ return Mathf.RoundToInt(roomRect.xMax); }
	}

	/// <summary>
	/// Whether the tile is inside the room
	/// </summary>
	public bool ContainsTile(Vector2 tile){
		if(tile.x >= roomLeft && tile.x <= roomRightTile && tile.y >= roomBotTile && tile.y <= roomTop){
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Sets the room rect.
	/// </summary>
	/// <param name="left">Left-most tile.</param>
	/// <param name="top">Top-most tile.</param>
	/// <param name="width">Width e.g. 9 if there are 8 spaces the player can be</param>
	/// <param name="height">Height e.g. 9 if there are 8 spaces the player can be</param>
	public void SetRect(float left, float top, float width, float height){
		// Rect objects use a GUI coordinate system where the y axis is opposite than in 3D view
		// So we actually pass in (left, BOT, width, height) instead of (left, TOP...)
		roomRect = new Rect(left, top-height, width, height);
	}

	public void LogRoomInfo(){
		Debug.Log("Room Center = (" + roomCenter.x + ", " + roomCenter.y + 
		          ")   Tile Dimensions = " + roomTileDimensions.x + " x " + roomTileDimensions.y);
		Debug.Log ("Room Bounds: (" + roomLeft + ", " + roomTop + ") to (" + roomRight + ", " + roomBot + ")");
	}
}
