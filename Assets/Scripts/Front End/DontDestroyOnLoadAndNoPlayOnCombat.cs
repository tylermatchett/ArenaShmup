using UnityEngine;
using System.Collections;

public class DontDestroyOnLoadAndNoPlayOnCombat : MonoBehaviour {

    public bool DestroyOnLoad = false;
    AudioSource audio;

	void Start () {
        audio = GetComponent<AudioSource>();
        if (!DestroyOnLoad)
            DontDestroyOnLoad(gameObject);
	}
	
	void Update () {
        if (GameObject.FindGameObjectsWithTag("MatchManager").Length > 0) {
            // Stop playing the music
            if (audio.isPlaying) {
                audio.Stop();
            }
        } else {
            if (!audio.isPlaying) {
                audio.Play();
            }
        }
	}
}
