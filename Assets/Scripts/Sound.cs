using UnityEngine;
using System.Collections.Generic;

public static class Sound {

	private static GameObject _cam;
	private static GameObject cam {
		get {
			if(_cam == null) {
				_cam = GameObject.Find("SpecialCamera");
			}
			return _cam;
		}
	}

	public static void PlaySound(AudioClip sound) {
		Vector3 point;
		if(cam != null)
			point = cam.transform.position;
		else
			point = new Vector3(0,0,0);
		AudioSource.PlayClipAtPoint(sound, point);
		// TODO: add volume adjustment from options
	}

	public static void PauseMusic() {
		if(cam == null) {
			Debug.LogError("An object named SpecialCamera could not be found in the scene.");
			return;
		}
		cam.GetComponent<MusicManagerScript>().PauseMusic();
	}

	public static void PlayMusic() {
		if(cam == null) {
			Debug.LogError("An object named SpecialCamera could not be found in the scene.");
			return;
		}
		cam.GetComponent<MusicManagerScript>().PlayMusic();
	}

	public static void StopMusic() {
		if(cam == null) {
			Debug.LogError("An object named SpecialCamera could not be found in the scene.");
			return;
		}
		cam.GetComponent<MusicManagerScript>().StopMusic();
	}

}
