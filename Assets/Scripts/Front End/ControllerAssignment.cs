using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;

public class ControllerAssignment : MonoBehaviour {

	public GameObject player1panel;
	public GameObject player2panel;
	public GameObject player3panel;
	public GameObject player4panel;

	PlayerPanelManagment playerPanel;

	// used for backing out to Main Menu
	bool BackingOut = false;
	float timerToBackOutCounter = 0f;

	// Update is called once per frame
	void Update () {
		// Get the active device
		// check for the start button to be pressed
		InputDevice device = InputManager.ActiveDevice;

		if (device.MenuWasPressed) {
			if (!GameManager.Instance.DeviceLocked(device)) {
				// Create a player and give them the active device
				// Assign that player to correct panel script based on player number
				int pn = getPlayerNumber();
				Player player = new Player(pn, device);
				switch (player.PlayerNumber) {
				case 0:
					playerPanel = player1panel.GetComponent<PlayerPanelManagment>();
					break;
				case 1:
					playerPanel = player2panel.GetComponent<PlayerPanelManagment>();
					break;
				case 2:
					playerPanel = player3panel.GetComponent<PlayerPanelManagment>();
					break;
				case 3:
					playerPanel = player4panel.GetComponent<PlayerPanelManagment>();
					break;
				default:
					Debug.LogError("LogError: Too Many Players.");
					break;
				}

				GameManager.Instance.playerList[pn] = player;
				playerPanel.player = player;
				playerPanel.state = PlayerPanelManagment.PanelState.CharacterSelect;
				playerPanel.StateUpdate();
			}
		}

		if ((device.Action2.IsPressed) && (!GameManager.Instance.DeviceLocked (device))) {
			BackingOut = true;
		} else {
			BackingOut = false;
		}

		if (BackingOut) {
			timerToBackOutCounter += Time.deltaTime;
			if (timerToBackOutCounter > 1f) {
				GameManager.Instance.LoadState("main_menu");
			}
		} else {
			timerToBackOutCounter = 0f;
		}
	}

	protected int getPlayerNumber() {
		for (int i = 0; i < GameManager.Instance.playerNumberList.Length; i++) {
			if (!GameManager.Instance.playerNumberList[i]) {
				GameManager.Instance.playerNumberList[i] = true;
				return i;
			}
		}

		Debug.LogError("LogError: Player number out of bounds");
		return -1;
	}
}
