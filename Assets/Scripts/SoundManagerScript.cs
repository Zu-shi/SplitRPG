using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundManagerScript : _Mono {

	[Tooltip("This is a list of all the AudioClips that have music in them and should be played. Music is selected at random from this list.")]
	public AudioClip[] Music;
	public AudioClip musicClip;

	private List<AudioClip> musicClips;
	private bool paused = false;  // Indicates if the music has been manually paused
	private bool stopped = false; // Indicates if the music has been manually stopped
	private bool fading = false;
	private bool stopOnFade = false;
	private float fadeTime = 0;
	private float fadeRate = 0; // volume / sec
	private float maxVolume = 1;

	private bool hasFocus = true;

	public bool isPlaying {
		get{return audio.isPlaying;}
	}

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
		maxVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
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
		Debug.Log ("SoundManager PlayMusic()");
		audio.clip = musicClip;
		audio.Play();
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

	public void PlaySound (AudioClip sound) {
		audio.PlayOneShot(sound, PlayerPrefs.GetFloat("SoundEffectsVolume", 1));
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

		bool dontNeedNewSong = stopped || paused || audio.isPlaying;

		if(!dontNeedNewSong && musicClips.Count != 0){
			audio.clip = musicClips[Random.Range(0, musicClips.Count)];
			if(audio.clip != null)
				audio.Play();
		}
		
		//if () AudioListener.pause = true; 
		//else if (AudioListener.pause) AudioListener.pause = false; 
		//else return;
	}

}
