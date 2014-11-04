using UnityEngine;
using System.Collections;

public class MainMenuScript : _Mono {

	public GUISkin skin;

	private enum State {
		Menu,
		StartGame,
		LoadGame,
		Options,
		QuitGame,
		NONE
	}

	private State state;
	private float oldVolume;

	GameObject cloud;
	FaderScript fader;
	SoundManagerScript soundManager;

	public void Start () {
		this.state = State.Menu;
		cloud = GameObject.Find ("BGCloud");
		fader = GameObject.Find("Fader").GetComponent<FaderScript>();
		fader.guiAlpha = 1;
		fader.fadeRate = .01f;
		fader.FadeUp(null);

		soundManager = gameObject.GetComponent<SoundManagerScript>();
	}

	public void Update(){
		cloud.renderer.material.mainTextureOffset += new Vector2(0, .00015f);
	}

	public void OnGUI () {
		GUI.skin = skin;
		GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		switch (state)
		{
		case State.Menu:
			OnMenu();
			break;
		case State.StartGame:
			OnStartGame();
			break;
		case State.LoadGame:
			OnLoadGame();
			break;
		case State.Options:
			OnOptions();
			break;
		case State.QuitGame:
			OnQuitGame();
			break;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}


	// ----------------------
	// OnGUI helper functions
	// ----------------------


	private void OnMenu () {
		GUILayout.BeginHorizontal(GUILayout.Width(150));
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical("box", GUILayout.MaxWidth(Screen.width / 3));
		GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
		//GUIStyle currentStyle = new GUIStyle( GUI.skin.box );
		//currentStyle.normal.background = MakeTex( 2, 2, new Color( 0f, 0f, 0f, 0.5f ) );

		if( GUILayout.Button( "Start Game") ) {
			state = State.StartGame;
		}
		GUILayout.Space(20);
		if( GUILayout.Button( "Load Game") ) {
			state = State.LoadGame;
		}
		GUILayout.Space(20);
		if( GUILayout.Button( "Options") ) {
			soundFX = PlayerPrefs.GetFloat("SoundEffectsVolume", 1);
			musicFX = PlayerPrefs.GetFloat("MusicVolume", 1);
			state = State.Options;
		}
		GUILayout.Space(20);
		if( GUILayout.Button( "Quit Game") ) {
			state = State.QuitGame;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void OnStartGame () {
		GUILayout.BeginHorizontal(GUILayout.Width(160));
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical("box", GUILayout.MaxWidth(Screen.width / 3));
		GUILayout.Label("Are you sure you want to start a new game?");
		GUILayout.Space(6);
		if(GUILayout.Button("Start")) {
			PlayerPrefs.SetString("LoadGame", "false");
			fader.FadeDown(LoadMainScene);
		}
		GUILayout.Space(6);
		if(GUILayout.Button("Back")) {
			state = State.Menu;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void LoadMainScene(){
		Application.LoadLevel( "Main" );
	}

	private void OnLoadGame () {
		GUILayout.BeginHorizontal(GUILayout.Width(160));
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical("box", GUILayout.MaxWidth(Screen.width / 3));
		GUILayout.Label("Are you sure you want to load your saved game?");
		GUILayout.Space(6);
		if(GUILayout.Button("Load")) {
			PlayerPrefs.SetString("LoadGame", "true");
			fader.FadeDown(LoadMainScene);
		}
		GUILayout.Space(6);
		if(GUILayout.Button("Back")) {
			state = State.Menu;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private float soundFX, musicFX;
	private Vector2 scrollPos = new Vector2(0,0);
	private int _optionsHeight = 20;
	private void OnOptions() {
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical("box", GUILayout.MaxWidth(300));
		
		GUILayout.Label("Options");
		
		GUILayout.Label("Resolution");
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.MaxWidth(150), GUILayout.MinHeight(100));
		foreach(Resolution res in Screen.resolutions) {
			if(GUILayout.Button("" + res.width + "x" + res.height)) {
				Screen.SetResolution(res.width, res.height, Screen.fullScreen);
			}
		}
		GUILayout.EndScrollView();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
		
		GUILayout.BeginVertical();
		GUILayout.Label("Sound Effects", GUILayout.Height(20));
		GUILayout.Label("Music", GUILayout.Height(20));
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical();
		soundFX = GUILayout.HorizontalSlider(soundFX, 0, 1, GUILayout.Width(120), GUILayout.Height(_optionsHeight));
		musicFX = GUILayout.HorizontalSlider(musicFX, 0, 1, GUILayout.Width(120), GUILayout.Height(_optionsHeight));
		gameObject.GetComponent<SoundManagerScript>().volume = musicFX;
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical();
		GUILayout.Label(soundFX.ToString("F2"), GUILayout.Width(55), GUILayout.Height(_optionsHeight));
		GUILayout.Label(musicFX.ToString("F2"), GUILayout.Width(55), GUILayout.Height(_optionsHeight));
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Apply", GUILayout.MaxWidth(150))) {
			PlayerPrefs.SetFloat("SoundEffectsVolume", soundFX);
			PlayerPrefs.SetFloat("MusicVolume", musicFX);
			gameObject.GetComponent<SoundManagerScript>().volume = musicFX;
			state = State.Menu;
		}
		if(GUILayout.Button("Back", GUILayout.MaxWidth(150))) {
			state = State.Menu;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void OnQuitGame () {
		GUILayout.BeginHorizontal(GUILayout.Width(160));
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical("box", GUILayout.MaxWidth(Screen.width / 3));
		GUILayout.Label("Are you sure you want to quit?");
		GUILayout.Space(6);
		if(GUILayout.Button("Quit")) {
			Application.Quit();
		}
		GUILayout.Space(6);
		if(GUILayout.Button("Back")) {
			state = State.Menu;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
}
