using UnityEngine;
using System.Collections;

public class MainMenuScript : _Mono {
	private enum State {
		Menu,
		StartGame,
		LoadGame,
		Options,
		QuitGame,
		NONE
	}

	private State state;

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
	}

	// ----------------------
	// OnGUI helper functions
	// ----------------------

	private void StartCentering () {
		GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
	}

	private void EndCentering () {
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}

	private void CenteredLabel(string toDisplay) {
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label(toDisplay);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void OnMenu () {
		StartCentering();
		CenteredLabel("Main Menu");
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		if( GUILayout.Button( "Start Game" ) ) {
			state = State.StartGame;
		}
		if( GUILayout.Button( "Load Game" ) ) {
			state = State.LoadGame;
		}
		if( GUILayout.Button( "Options" ) ) {
			state = State.Options;
		}
		if( GUILayout.Button( "Quit Game" )) {
			state = State.QuitGame;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		EndCentering();
	}

	private void OnStartGame () {
		StartCentering();
		CenteredLabel("Start Game");
		CenteredLabel("Are you sure you want to start a new game?");
		CenteredLabel("This will overwrite any existing save.");;
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		if(GUILayout.Button("Start")) {
			PlayerPrefs.SetString("LoadGame", "false");
			fader.FadeDown(LoadMainScene);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		if(GUILayout.Button("Back")) {
			state = State.Menu;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		EndCentering();
	}

	private void LoadMainScene(){
		Application.LoadLevel( "Main" );
	}

	private void OnLoadGame () {
		StartCentering();
		CenteredLabel("Load Game");
		CenteredLabel("Are you sure you want to load your saved game?");
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		if(GUILayout.Button("Load")) {
			PlayerPrefs.SetString("LoadGame", "true");
			fader.FadeDown(LoadMainScene);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		if(GUILayout.Button("Back")) {
			state = State.Menu;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		EndCentering();
	}

	private float soundFX, musicFX;
	private Vector2 scrollPos = new Vector2(0,0);
	private int _optionsHeight = 20;
	private void OnOptions () {
		StartCentering();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Options");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Resolution");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
		scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.MaxWidth(150), GUILayout.MinHeight(100));
		foreach(Resolution res in Screen.resolutions) {
			if(GUILayout.Button("" + res.width + "x" + res.height)) {
				Screen.SetResolution(res.width, res.height, Screen.fullScreen);
			}
		}
		GUILayout.EndScrollView();
		GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Audio");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginVertical();
		GUILayout.Label("Sound Effects", GUILayout.Height(20));
		GUILayout.Label("Music", GUILayout.Height(20));
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical();
		soundFX = GUILayout.HorizontalSlider(soundFX, 0, 100, GUILayout.Width(120), GUILayout.Height(_optionsHeight));
		musicFX = GUILayout.HorizontalSlider(musicFX, 0, 100, GUILayout.Width(120), GUILayout.Height(_optionsHeight));
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical();
		GUILayout.Label("" + (int)soundFX, GUILayout.Width(30), GUILayout.Height(_optionsHeight));
		GUILayout.Label("" + (int)musicFX, GUILayout.Width(30), GUILayout.Height(_optionsHeight));
		GUILayout.EndVertical();
		
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
		if(GUILayout.Button("Apply", GUILayout.MaxWidth(150))) {
			PlayerPrefs.SetInt("SoundEffectsVolume", (int)soundFX);
			PlayerPrefs.SetInt("MusicVolume", (int)musicFX);
			state = State.Menu;
		}
		if(GUILayout.Button("Back", GUILayout.MaxWidth(150))) {
			state = State.Menu;
		}
		GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
		EndCentering();
	}

	private void OnQuitGame () {
		StartCentering();
		CenteredLabel("Are you sure you want to quit?");
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Quit")) {
			Application.Quit();
		}
		if(GUILayout.Button("Back")) {
			state = State.Menu;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		EndCentering();
	}
}
