using UnityEngine;
using System.Collections;

public class Globals {

	// Width and height of one side of the screen in tiles
	public const int SIDEWIDTH = 9;
	public const int SIDEHEIGHT = 10;

	public const int PIXELS_PER_TILE = 32;

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


}
