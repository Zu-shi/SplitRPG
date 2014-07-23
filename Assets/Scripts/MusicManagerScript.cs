using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MusicManagerScript : _Mono {

	[Tooltip("This is a list of all the AudioClips that have music in them and should be played. Music is selected at random from this list.")]
	public AudioClip[] Music;

	private List<AudioClip> musicClips;
	private bool paused = false;  // Indicates if the music has been manually paused
	private bool stopped = false; // Indicates if the music has been manually stopped
	private bool fading = false;
	private bool stopOnFade = false;
	private float fadeTime = 0;
	private float fadeRate = 0;
	private float maxVolume = 1;

	public float volume {
		get{
			return audio.volume;
		}
		set{
			maxVolume = value;
			audio.volume = value;
		}
	}

	public void Start () {
		maxVolume = PlayerPrefs.GetInt("MusicVolume", 100) /100.0f;
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
		stopped = true;
	}

	public void PlayMusic() {
		if(paused) {
			audio.Play();
		}
		audio.volume = maxVolume;
		stopped = false;
		paused = false;
	}

	public void FadeOutMusic(float seconds, bool stopMusic = false) {
		fading = true;
		fadeTime = seconds;
		stopOnFade = stopMusic;
		fadeRate = maxVolume / seconds;
	}

	public void Update() {
		if(fading) {
			fadeTime -= Time.deltaTime;
			audio.volume -= fadeRate * Time.deltaTime;
			if(fadeTime <= 0) {
				fading = false;
				if(stopOnFade) {
					StopMusic();
				} else {
					PauseMusic();
				}
			}

		}
		if(stopped || paused || audio.isPlaying) { // We were told to stop playing or the current clip is still playing
			return;
		}
		if(musicClips.Count != 0){
			audio.clip = musicClips[Random.Range(0, musicClips.Count)];
			if(audio.clip != null)
				audio.Play();
		}

	}
}
