using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MusicManagerScript : _Mono {

	public AudioClip[] Music;

	private List<AudioClip> musicClips;
	private bool paused = false;  // Idicates if the music has been manually paused or stopped.

	public void Start () {
		musicClips = new List<AudioClip>();
		foreach(AudioClip ac in Music)
			musicClips.Add(ac);
	}

	public void AddMusicClip(AudioClip clip){
		musicClips.Add(clip);
	}

	public void ClearMusicClips(){
		musicClips.Clear();
	}
	
	public void PauseMusic() {
		audio.Pause();
		paused = true;
	}

	public void StopMusic() {
		audio.Stop();
		paused = true;
	}

	public void PlayMusic() {
		audio.Play();
		paused = false;
	}

	public void Update(){
		if(paused || audio.isPlaying) { // We were told to stop playing or the current clip is still playing
			return;
		}
		audio.clip = musicClips[Random.Range(0, musicClips.Count)];
		if(audio.clip != null)
			audio.Play();
	}
}
