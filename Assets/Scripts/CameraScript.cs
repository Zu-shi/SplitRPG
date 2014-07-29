using UnityEngine;
using System.Collections;

public class CameraScript : _Mono {

	public const float TRANSITION_SPEED = 14f;
	public const float CUTSCENE_PAN_SPEED = 6f;

	// Currently in gameplay mode / following player
	bool gameplayCamera{get; set;}

	// Center - The location of the camera without the offset
	public Vector2 center{get; set;}

	// Offset - Shifts the final position of the camera, used for shakycam 
	public Vector2 offset{get; set;}

	// Camera bounds in current room
	Rect bounds;

	Action currShakeAction;
	Action currPanAction;
	Action currFadeAction;

	[Tooltip("Prefab of 'fader' object which controls fading the camera in and out.")]
	public GameObject faderPrefab;

	FaderScript _fader;
	public FaderScript fader{get {return _fader;} }

	GameObject _player;
	public GameObject player{get{return _player;}}

	RoomManagerScript roomManager;


	void Start () {

		roomManager = Globals.roomManager;

		// Instantiate a fader object for this camera
		GameObject faderObject = (GameObject)Instantiate(faderPrefab);
		faderObject.layer = gameObject.layer;
		_fader = faderObject.GetComponent<FaderScript>();

		// Find the player on our side of the screen
		_player = (LayerMask.NameToLayer("Right") == gameObject.layer ? 
		          Globals.playerRight.gameObject : Globals.playerLeft.gameObject);

		gameplayCamera = true;
		currPanAction = currShakeAction = currFadeAction = null;

		center = new Vector2(x, y);
		offset = new Vector2(0,0);

	}

	// This is just a quick test using callback functions
	// Real cutscenes should use a more robust system
	public void RunCutsceneTest(){
		BeginCutscene();
		fader.guiAlpha = 1;
		BeginFadeUp(CutsceneTest2);
	}
	void CutsceneTest2(){
		BeginCutscenePan(30, 10, CutsceneTest3);
		BeginShakyCam(2f);
	}
	void CutsceneTest3(){
		BeginCutscenePan(0, 0, BeginFadeDown);
	}

	void Update () {
		if(gameplayCamera){
			GameplayModeUpdate();
		}

		// Update camera location based on center + offset
		x = center.x + offset.x;
		y = center.y + offset.y;
	}

	void GameplayModeUpdate(){
		// Follow the player
		CenterOnPlayer();
	}

	public void CenterOnPlayer(){
		center = CalculateFollowPosition(player);
	}

	// These are methods so that they can be used as callbacks
	public void DisableGameplayMode(){
		gameplayCamera = false;
	}
	public void EnableGameplayMode(){
		gameplayCamera = true;
	}

	/// <summary>
	/// Calculate bounds of camera movement based on current room
	/// </summary>
	void UpdateGameplayCameraBounds(){
		float rcx = roomManager.roomCenter.x;
		float rcy = roomManager.roomCenter.y;

		float sw = Globals.SIDEWIDTH;
		float sh = Globals.SIDEHEIGHT;

		// Update bounds of camera
		bounds.xMin = roomManager.roomLeft + sw / 2;
		bounds.xMin = Mathf.Min (bounds.xMin, rcx);
		bounds.xMax = roomManager.roomRight - sw / 2;
		bounds.xMax = Mathf.Max (bounds.xMax, rcx);

		bounds.yMax = roomManager.roomTop - sh / 2;
		bounds.yMax = Mathf.Max (bounds.yMax, rcy);
		bounds.yMin = roomManager.roomBot + sh / 2;
		bounds.yMin = Mathf.Min (bounds.yMin, rcy);

//		Debug.Log (bounds.xMin + ", " + bounds.xMax + ", " + bounds.yMin + ", " + bounds.yMax);
	}
	
	/// <summary>
	/// Returns the position of the camera that is closest to the object
	/// without going out of bounds of the room
	/// </summary>
	public Vector2 CalculateFollowPosition(GameObject target){
		UpdateGameplayCameraBounds();

		float ox = target.transform.position.x;
		float oy = target.transform.position.y;

		float fx = Utils.Clamp(ox, bounds.xMin, bounds.xMax);
		float fy = Utils.Clamp(oy, bounds.yMin, bounds.yMax);

		Vector2 fp = new Vector2(fx, fy);
		return fp;
	}

	public void BeginNewRoomTransitionPan(Utils.VoidDelegate callback){
		if(currPanAction != null)
			currPanAction.Destroy();

		gameplayCamera = false;

		// Calculate where the camera wants to be when the player walks in
		Vector2 startPos = CalculateFollowPosition(player);
		currPanAction = CameraPanTransitionAction.Create(this, startPos, callback + EnableGameplayMode).StartAction();
	}

	/// <summary>
	/// Fade out, then fade in on the new room
	/// </summary>
	public void BeginRoomTransitionFade(Utils.VoidDelegate callback){
		gameplayCamera = false;
		FadeTransition(CenterOnPlayer, callback + EnableGameplayMode);
	}

	public void FadeTransition(Utils.VoidDelegate middleCode, Utils.VoidDelegate callback){
		if(currFadeAction != null)
			currFadeAction.Destroy();
		currFadeAction = CameraFadeTransitionAction.Create(this, middleCode, callback).StartAction();
	}

	/// <summary>
	/// Enters cutscene mode, making the camera ready for cutscene commands
	/// </summary>
	public void BeginCutscene(){
		gameplayCamera = false;
	}

	/// <summary>
	/// Begins a pan in a cutscene
	/// </summary>
	/// <param name="destX">Destination x.</param>
	/// <param name="destY">Destination y.</param>
	/// <param name="callback">Callback.</param>
	public void BeginCutscenePan(float destX, float destY, Utils.VoidDelegate callback){
		if(currPanAction != null)
			Destroy (currPanAction.gameObject);
		currPanAction = CameraPanTransitionAction.
			Create(this, new Vector2(destX, destY), CUTSCENE_PAN_SPEED, callback).StartAction();
	}

	/// <summary>
	/// Begins the fade up.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void BeginFadeUp(Utils.VoidDelegate callback){
		fader.FadeUp(callback);
	}
	/// <summary>
	/// Begins the fade up.
	/// </summary>
	public void BeginFadeUp(){
		BeginFadeUp(null);
	}

	/// <summary>
	/// Begins the fade down.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void BeginFadeDown(Utils.VoidDelegate callback){
		fader.FadeDown(callback);
	}

	/// <summary>
	/// Begins the fade down.
	/// </summary>
	public void BeginFadeDown(){
		BeginFadeDown(null);
	}

	/// <summary>
	/// Begins shaky cam.
	/// </summary>
	public void BeginShakyCam(){
		if(currShakeAction != null)
			currShakeAction.Destroy();
		currShakeAction = CameraShakeAction.Create(this, 9999, null).StartAction();
	}

	/// <summary>
	/// Begins shaky cam.
	/// </summary>
	/// <param name="duration">Duration.</param>
	public void BeginShakyCam(float duration){
		if(currShakeAction != null)
			currShakeAction.Destroy();
		currShakeAction = CameraShakeAction.Create(this, duration, ShakyCamEndedSelf).StartAction();
	}
	
	void ShakyCamEndedSelf(){
		currShakeAction = null;
		offset = new Vector2(0,0);
	}

	/// <summary>
	/// Ends the shaky cam.
	/// </summary>
	public void EndShakyCam(){
		currShakeAction.Finish();
		ShakyCamEndedSelf();
	}
	
	/// <summary>
	/// Helper method for panning
	/// XYPan means it pans the X and Y coordinates separately, so it isn't very cinematic 
	/// </summary>
	public void XYPanTo(float targetX, float py, float speed){
		if(center.x == targetX && center.y == py){
			return;
		}

		// Pan center towards (px, py)
		float ms = speed * Time.deltaTime;
		float tx, ty;
		tx = Utils.MoveValueTowards(center.x, targetX, ms);
		ty = Utils.MoveValueTowards(center.y, py, ms);
		center = new Vector2(tx, ty);
	}
	
}
