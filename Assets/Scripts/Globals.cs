using UnityEngine;
using System.Collections;

public class Globals {
	
	/// <summary>
	/// Width of one side in tiles
	/// </summary>
	public const int SIDEWIDTH = 18;

	/// <summary>
	/// Height of one side in tiles
	/// </summary>
	public const int SIDEHEIGHT = 20;

	/// <summary>
	/// Number of pixels per tile (assuming you imported from Tiled)
	/// </summary>
	public const int PIXELS_PER_TILE = 64;

	static PlayerControllerScript _playerLeft = null;
	public static PlayerControllerScript playerLeft{
		get{
			if(_playerLeft == null)
				_playerLeft = GameObject.Find("PlayerLeft").GetComponent<PlayerControllerScript>();
			return _playerLeft;
		}
	}

	static PlayerControllerScript _playerRight = null;
	public static PlayerControllerScript playerRight{
		get{
			if(_playerRight == null)
				_playerRight = GameObject.Find("PlayerRight").GetComponent<PlayerControllerScript>();
			return _playerRight;
		}
	}

	static CameraScript _cameraLeft = null;
	public static CameraScript cameraLeft{
		get{
			if(_cameraLeft == null)
				_cameraLeft = GameObject.Find("CameraLeft").GetComponent<CameraScript>();
			return _cameraLeft;
		}
	}
	
	static CameraScript _cameraRight = null;
	public static CameraScript cameraRight{
		get{
			if(_cameraRight == null)
				_cameraRight = GameObject.Find("CameraRight").GetComponent<CameraScript>();
			return _cameraRight;
		}
	}

	static GameManagerScript _gameManager = null;
	public static GameManagerScript gameManager{
		get{
			if(_gameManager == null)
				_gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
			return _gameManager;
		}
	}

	static RoomManagerScript _roomManager = null;
	public static RoomManagerScript roomManager{
		get{
			if(_roomManager == null)
				_roomManager = GameObject.Find("RoomManager").GetComponent<RoomManagerScript>();
			return _roomManager;
		}
	}

	static HeightSorterScript _heightSorter = null;
	public static HeightSorterScript heightSorter{
		get{
			if(_heightSorter == null)
				_heightSorter = GameObject.Find("HeightSorter").GetComponent<HeightSorterScript>();
			return _heightSorter;
		}
	}

	static CollisionManagerScript _collisionManager = null;
	public static CollisionManagerScript collisionManager{
		get{
			if(_collisionManager == null)
				_collisionManager = GameObject.Find("CollisionManager").GetComponent<CollisionManagerScript>();
			return _collisionManager;
		}
	}


}
