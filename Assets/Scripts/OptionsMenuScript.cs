using UnityEngine;
using System.Collections;

public class OptionsMenuScript : MonoBehaviour {

	public AudioClip startSound;
	public GUISkin optionSkin;

	private enum State {
		NONE,
		MENU,
		OPTIONS,
		QUIT
	}

	private State s = State.NONE;

	public void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(s == State.NONE) {
				Pause();
			}
			else {
				Pause(false);
			}
		}
	}

	public void OnGUI() {
		GUI.skin = optionSkin;
		GUILayout.BeginArea(new Rect(0,-30,Screen.width,Screen.height));
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();

		switch(s){
		case State.NONE:
			OnNone();
			break;
		case State.MENU:
			OnMenu();
			break;
		case State.OPTIONS:
			OnOptions();
			break;
		case State.QUIT:
			OnQuit();
			break;
		}

		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();

	}

	private void Pause(bool pause = true) {
		Globals.soundManager.PlaySound (startSound);

		if(pause){
			s = State.MENU;
			GameObject.Find("PlayerLeft").GetComponent<PlayerControllerScript>().enabled = false;
			GameObject.Find("PlayerRight").GetComponent<PlayerControllerScript>().enabled = false;
			GameObject.Find("GameManager").GetComponent<PlayerInputScript>().enabled = false;
		} else {
			s = State.NONE;
			GameObject.Find("PlayerLeft").GetComponent<PlayerControllerScript>().enabled = true;
			GameObject.Find("PlayerRight").GetComponent<PlayerControllerScript>().enabled = true;
			GameObject.Find("GameManager").GetComponent<PlayerInputScript>().enabled = true;
		}
	}

	private void OnNone() {
		return;
	}

	private void OnMenu() {
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Menu");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
		if(GUILayout.Button("Save Game", GUILayout.MaxWidth(150))) {
			Debug.Log("Game saves automatically.");
		}
		GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
		if(GUILayout.Button("Load Game", GUILayout.MaxWidth(150))) {
			Debug.Log("Loading serialized game.");
			Pause(false);
			Globals.levelManager.LoadSerializedGame();
		}
		GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
		if(GUILayout.Button("Options", GUILayout.MaxWidth(150))) {
			soundFX = PlayerPrefs.GetInt("SoundEffectsVolume", 100);
			musicFX = PlayerPrefs.GetInt("MusicVolume", 100);
			s = State.OPTIONS;
		}
		GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Back to Game", GUILayout.MaxWidth(150))) {
			Pause(false);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Quit Game", GUILayout.MaxWidth(150))) {
			s = State.QUIT;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private float soundFX, musicFX;
	private Vector2 scrollPos = new Vector2(0,0);
	private int _optionsHeight = 20;
	private void OnOptions() {
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
			s = State.MENU;
		}
		if(GUILayout.Button("Back", GUILayout.MaxWidth(150))) {
			s = State.MENU;
		}
		GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
	}

	private void OnQuit() {
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Are you sure you want to quit?");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Quit", GUILayout.MaxWidth(150))) {
			Application.Quit();
		}
		if(GUILayout.Button("Back", GUILayout.MaxWidth(150))) {
			s = State.MENU;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
}
