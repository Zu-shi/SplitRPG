using UnityEngine;
using System.Collections;

public class CameraScript : _Mono {

	enum Mode {
		GAMEPLAY, TRANSITION, FADETRANSITION, CUTSCENE
	}

	Mode mode;


	// Center - The location of the camera without the offset
	public Vector2 center{get; set;}

	// Offset - Shifts the final position of the camera, used for shakycam 
	public Vector2 offset{get; set;}

	// Camera bounds in current room
	Rect bounds;
	
	// Panning between rooms
	Vector2 transitionDest;
	float transitionSpeed;
	Utils.VoidDelegate transitionCallback;

	// Panning in cutscenes
	Vector2 panDest;
	float panSpeed;
	Utils.VoidDelegate panCallback;

	// Camera shake
	Vector2 shakeDest;
	float shakeSpeed;
	bool isShaking;

	[Tooltip("Prefab of 'fader' object which controls fading the camera in and out.")]
	public GameObject faderPrefab;
	FaderScript fader;

	GameObject player;
	RoomManagerScript roomManager;


	void Start () {

		roomManager = Globals.roomManager;

		// Instantiate a fader object for this camera
		GameObject faderObject = (GameObject)Instantiate(faderPrefab);
		faderObject.layer = gameObject.layer;
		fader = faderObject.GetComponent<FaderScript>();

		// Find the player on our side of the screen
		player = (LayerMask.NameToLayer("Right") == gameObject.layer ? 
		          Globals.playerRight.gameObject : Globals.playerLeft.gameObject);

		mode = Mode.GAMEPLAY;
		isShaking = false;

		transitionSpeed = 10f;
		panSpeed = 3f;

		center = new Vector2(x, y);

		shakeDest = new Vector2(.07f, 0);
		shakeSpeed = 8f;
	}

	// This is just a quick test using callback functions
	// Real cutscenes should use a more robust system
	public void RunCutsceneTest(){
		BeginCutscene();
		fader.guiAlpha = 1;
		BeginFadeUp(CutsceneTest2);
	}
	void CutsceneTest2(){
		BeginCutscenePan(10, 5, CutsceneTest3);
		BeginShakyCam(2f);
	}
	void CutsceneTest3(){
		BeginCutscenePan(0, .5f, BeginFadeDown);
	}

	void Update () {
	
		switch(mode){
		case Mode.GAMEPLAY:
			GameplayModeUpdate();
			break;
		case Mode.TRANSITION:
			TransitionModeUpdate();
			break;
		case Mode.FADETRANSITION:
			break;
		case Mode.CUTSCENE:
			CutsceneModeUpdate();
			break;
		}

		if(isShaking){
			UpdateShake();
		}

		// Update camera location based on center + offset
		x = center.x + offset.x;
		y = center.y + offset.y;
	}

	void GameplayModeUpdate(){
		// Follow the player
		center = CalculateFollowPosition(player);
	}

	// Calculate bounds of camera movement based on current room
	void UpdateGameplayCameraBounds(){
		float rcx = roomManager.roomCenter.x;
		float rcy = roomManager.roomCenter.y;

		float sw = Globals.SIDEWIDTH;
		float sh = Globals.SIDEHEIGHT;

		// Update bounds of camera
		bounds.xMin = roomManager.roomLeft -.5f + sw / 2;
		bounds.xMin = Mathf.Min (bounds.xMin, rcx);
		bounds.xMax = roomManager.roomRight +.5f - sw / 2;
		bounds.xMax = Mathf.Max (bounds.xMax, rcx);
		bounds.yMin = roomManager.roomBot - .5f + sh / 2;
		bounds.yMin = Mathf.Min (bounds.yMin, rcy);
		bounds.yMax = roomManager.roomTop + .5f - sh / 2;
		bounds.yMax = Mathf.Max (bounds.yMax, rcy);

		//		Debug.Log (bounds.xMin + ", " + bounds.xMax + ", " + bounds.yMin + ", " + bounds.yMax);
	}

	// Returns the position of the camera that is closest to the object 
	// without going out of the bounds of the room
	Vector2 CalculateFollowPosition(GameObject obj){
		UpdateGameplayCameraBounds();

		float ox = obj.transform.position.x;
		float oy = obj.transform.position.y;

		float fx = Utils.Clamp(ox, bounds.xMin, bounds.xMax);
		float fy = Utils.Clamp(oy, bounds.yMin, bounds.yMax);

		return new Vector2(fx, fy);
	}

	void TransitionModeUpdate(){
		// Pan a bit towards the destination
		float dx = transitionDest.x;
		float dy = transitionDest.y;
		XYPanTo(dx, dy, transitionSpeed);

		// If we reached it we're done
		if(center.x == dx && center.y == dy){
			mode = Mode.GAMEPLAY;
			transitionCallback();
		}
	}

	// Pan from the camera's current location to the new room
	public void BeginRoomTransitionPan(Utils.VoidDelegate callback){
		mode = Mode.TRANSITION;
		transitionCallback = callback;

		// Calculate where the camera wants to be when the player walks in
		Vector2 startPos = CalculateFollowPosition(player);
		
		transitionDest = new Vector2(startPos.x, startPos.y);
		
	}

	// Fade out, then fade in on the new room
	public void BeginRoomTransitionFade(Utils.VoidDelegate callback){
		mode = Mode.FADETRANSITION;
		transitionCallback = callback;

		BeginFadeDown(MiddleOfFade);
	}

	void MiddleOfFade(){
		// Center on the player
		center = CalculateFollowPosition(player);

		BeginFadeUp(CompleteFade);
	}

	void CompleteFade(){
		mode = Mode.GAMEPLAY;
		if(transitionCallback != null)
			transitionCallback();
	}

	void CutsceneModeUpdate(){
		float dx = panDest.x;
		float dy = panDest.y;

		// Do panning if needed
		if(center.x != dx || center.y != dy){
			XYPanTo(dx, dy, panSpeed);
			
			if(center.x == dx && center.y == dy){
				if(panCallback != null)
					panCallback();
			}
		}

	}

	public void BeginCutscene(){
		mode = Mode.CUTSCENE;
	}

	public void BeginCutscenePan(float destX, float destY, Utils.VoidDelegate callback){
		panCallback = callback;
		panDest = new Vector2(destX, destY);
	}

	public void BeginFadeUp(Utils.VoidDelegate callback){
		fader.FadeUp(callback);
	}
	public void BeginFadeUp(){
		BeginFadeUp(null);
	}

	public void BeginFadeDown(Utils.VoidDelegate callback){
		fader.FadeDown(callback);
	}
	public void BeginFadeDown(){
		BeginFadeDown(null);
	}

	public void BeginShakyCam(){
		isShaking = true;
		StartNextShake();
	}

	public void BeginShakyCam(float duration){
		BeginShakyCam();
		CancelInvoke("EndShakyCam");
		Invoke ("EndShakyCam", duration);
	}

	public void EndShakyCam(){
		isShaking = false;
		offset = new Vector2(0,0);
	}

	void StartNextShake(){
		// Right now we just move the camera back and forth really fast
		shakeDest = new Vector2(-shakeDest.x, shakeDest.y);
	}

	void UpdateShake(){
		// Start the next shake if we reached shakeDest
		if(offset.x == shakeDest.x && offset.y == shakeDest.y){
			StartNextShake();
		}

		// Move the offset towards shakeDest
		float ss = shakeSpeed * Time.deltaTime;
		float tx, ty;
		tx = Utils.MoveValueTowards(offset.x, shakeDest.x, ss);
		ty = Utils.MoveValueTowards(offset.y, shakeDest.y, ss);
		offset = new Vector2(tx, ty);
	}

	// Helper method for panning
	// XYPan means it pans the X and Y coordinates separately, so it isn't very cinematic 
	void XYPanTo(float px, float py, float speed){
		if(center.x == px && center.y == py){
			return;
		}

		// Pan center towards (px, py)
		float ms = speed * Time.deltaTime;
		float tx, ty;
		tx = Utils.MoveValueTowards(center.x, px, ms);
		ty = Utils.MoveValueTowards(center.y, py, ms);
		center = new Vector2(tx, ty);
	}



}
