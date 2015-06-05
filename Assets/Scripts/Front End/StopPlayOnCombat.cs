using UnityEngine;
using System.Collections;

public class StopPlayOnCombat : MonoBehaviour {

    AudioSource audio;

    void Start() {
        audio = GetComponent<AudioSource>();
	}
	
    void Update() {
        if (GameObject.FindGameObjectsWithTag("MatchManager").Length > 0) {
            // Stop playing the music
            if (audio.isPlaying) {
                audio.Stop();
            }
        }
        else {
            if (!audio.isPlaying) {
                audio.Play();
            }
        }
	}
}
