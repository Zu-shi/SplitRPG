using UnityEngine;
using System.Collections;

public class CameraScript : _Mono {

	enum Mode {
		GAMEPLAY, TRANSITION, CUTSCENE
	}

	Mode mode;

	public Vector2 center{get; set;}

	Vector2 transitionDest;
	float transitionSpeed;

	Vector2 panDest;
	float panSpeed;

	public bool isMoving{get;set;}

	RoomManagerScript roomManager;

	Utils.VoidDelegate transitionCallback;
	Utils.VoidDelegate panCallback;

	public GameObject faderPrefab;
	FaderScript fader;

	void Start () {

		roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManagerScript>();

		GameObject faderObject = (GameObject)Instantiate(faderPrefab);
		faderObject.layer = gameObject.layer;
		fader = faderObject.GetComponent<FaderScript>();

		mode = Mode.GAMEPLAY;
		isMoving = false;

		transitionSpeed = 10f;
		panSpeed = 3f;


		Invoke ("CutsceneTest1", 1f);
	}

	void CutsceneTest1(){
		BeginCutscene();
		fader.guiAlpha = 1;
		BeginCutsceneFadeUp();
		BeginCutscenePan(10, 5, CutsceneTest2);
	}
	void CutsceneTest2(){
		BeginCutscenePan(0, .5f, BeginCutsceneFadeDown);
	}

	void Update () {
		switch(mode){
		case Mode.GAMEPLAY:
			GameplayModeUpdate();
			break;
		case Mode.TRANSITION:
			TransitionModeUpdate();
			break;
		case Mode.CUTSCENE:
			CutsceneModeUpdate();
			break;
		}

		// Center might not be needed...
		center = new Vector2(x, y);
	}

	void GameplayModeUpdate(){
		// follow player around big rooms
	}

	void TransitionModeUpdate(){
		float dx = transitionDest.x;
		float dy = transitionDest.y;

		XYPanTo(dx, dy, transitionSpeed);

		if(x == dx && y == dy){
			mode = Mode.GAMEPLAY;
			transitionCallback();
		}

	}

	void CutsceneModeUpdate(){

		// Do panning
		float dx = panDest.x;
		float dy = panDest.y;

		if(x != dx || y != dy){
			XYPanTo(dx, dy, panSpeed);
			
			if(x == dx && y == dy){
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

	public void BeginCutsceneFadeUp(){
		fader.targetAlpha = 0;
	}

	public void BeginCutsceneFadeDown(){
		fader.targetAlpha = 1;
	}

	public void BeginRoomTransitionPan(Utils.VoidDelegate callback){
		mode = Mode.TRANSITION;

		transitionCallback = callback;

		float rx = roomManager.roomCenter.x;
		float ry = roomManager.roomCenter.y;

		float rw = roomManager.roomSize.x;
		float rh = roomManager.roomSize.y;

		float dx = 0, dy = 0;

		if(rw <= Globals.SIDEWIDTH){
			dx = rx;
		} 

		if(rh <= Globals.SIDEHEIGHT){
			dy = ry;
		}

		transitionDest = new Vector2(dx, dy);

	}

	void XYPanTo(float px, float py, float speed){
		if(x == px && y == py){
			isMoving = false;
			return;
		} else {
			isMoving = true;
		}
		
		float ms = speed * Time.deltaTime;

		x = Utils.MoveValueTowards(x, px, ms);
		y = Utils.MoveValueTowards(y, py, ms);
	}



}
