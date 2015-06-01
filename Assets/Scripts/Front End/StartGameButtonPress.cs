using UnityEngine;
using System.Collections;
using InControl;

public class StartGameButtonPress : MonoBehaviour {
	public string sceneToLoad = "main_menu";

	void Update () {
		InputDevice device = InputManager.ActiveDevice;

		if (device.AnyButton.WasPressed || device.MenuWasPressed) {
			GameManager.Instance.LoadState(sceneToLoad);
		}
	}
}
