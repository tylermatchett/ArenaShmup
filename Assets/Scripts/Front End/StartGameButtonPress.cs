using UnityEngine;
using System.Collections;
using InControl;

public class StartGameButtonPress : MonoBehaviour {
	public string sceneToLoad = "main_menu";
    public AudioSource audio;

	void Update () {
		InputDevice device = InputManager.ActiveDevice;

        if (device.AnyButton.WasPressed || device.MenuWasPressed) {
            audio.Play();
            GameManager.Instance.LoadState(sceneToLoad);
		}
	}
}
