using UnityEngine;
using System.Collections;

public class PlayMenuSounds : MonoBehaviour {

    AudioSource audio;
    public AudioClip switch_sfx;
    public AudioClip select_sfx;
    public AudioClip back_sfx;

	void Start () {
        audio = GetComponent<AudioSource>();
	}
	
    public void PlaySwitchSFX() {
        audio.clip = switch_sfx;
        audio.Play();
    }

    public void PlaySelectSFX() {
        audio.clip = select_sfx;
        audio.Play();
    }

    public void PlayBackSFX() {
        audio.clip = back_sfx;
        audio.Play();
    }
}
