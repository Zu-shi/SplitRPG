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

	public void Start () {
		this.state = State.Menu;
		cloud = GameObject.Find ("BGCloud");
	}

	public void Update(){
		cloud.renderer.material.mainTextureOffset += new Vector2(0, .0004f);
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
		GUILayout.BeginArea( new Rect(0, Screen.height / 4, Screen.width, Screen.height) );
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
		Application.LoadLevel( "Main" );
	}

	private void OnLoadGame () {
		StartCentering();
		CenteredLabel("Load Game");
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		if(GUILayout.Button("Back")) {
			state = State.Menu;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		EndCentering();
	}

	private void OnOptions () {
		StartCentering();
		CenteredLabel("Options");
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		if(GUILayout.Button("Back")) {
			state = State.Menu;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
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
