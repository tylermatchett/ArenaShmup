using UnityEngine;
using InControl;
using System.Collections;

public class MenuManagement : MonoBehaviour {

    PlayMenuSounds menuSounds;

    void Start() {
        menuSounds = GameObject.FindGameObjectWithTag("fe_sfx").GetComponent<PlayMenuSounds>();
    }

    void Update() {
        InputDevice device = InputManager.ActiveDevice;

        if ((device.Direction.Up.WasPressed) || (device.Direction.Down.WasPressed)) {
            menuSounds.PlaySwitchSFX();
        }
    }

    public void LoadLevel(string level) {
        menuSounds.PlaySelectSFX();
		GameManager.Instance.LoadState(level);
	}

	public void ExitGame() {
		Application.Quit();
	}
}
