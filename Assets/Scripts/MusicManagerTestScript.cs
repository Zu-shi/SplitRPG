using UnityEngine;
using System.Collections;

public class MusicManagerTestScript : _Mono {
	
	// Use this for initialization
	public void Start () {
		Invoke("StopTest", 10);
		Invoke("PlayTest", 15);
		Invoke("PauseTest", 20);
		Invoke("PlayTest", 22);
		Invoke("FadeTest1", 27);
		Invoke("PlayTest", 33);
		Invoke("FadeTest2", 35);
		Invoke("PlayTest", 43);
		Invoke("VolumeTest1", 48);
		Invoke("VolumeTest2", 53);
		Invoke("VolumeTest3", 58);
		Invoke("VolumeTest2", 63);

	}

	private void PlayTest() {
		Debug.Log("Testing Sound.PlayMusic()");
		Sound.PlayMusic();
	}

	private void PauseTest() {
		Debug.Log("Testing Sound.PauseMusic()");
		Sound.PauseMusic();
	}

	private void StopTest() {
		Debug.Log("Testing Sound.StopMusic()");
		Sound.StopMusic();
	}

	private void FadeTest1() {
		Debug.Log("Testing Sound.FadeOutMusic()");
		Sound.FadeOutMusic(3);
	}

	private void FadeTest2() {
		Debug.Log("Testing Sound.FadeOutMusic()");
		Sound.FadeOutMusic(5, true);
	}

	private void VolumeTest1() {
		Debug.Log("Testing Sound.musicVolume = 0.5");
		Sound.musicVolume = 0.5f;
	}

	private void VolumeTest2() {
		Debug.Log("Testing Sound.musicVolume = 1");
		Sound.musicVolume = 1;
	}

	private void VolumeTest3() {
		Debug.Log("Testing Sound.musicVolume = 0");
		Sound.musicVolume = 0;
	}
}
