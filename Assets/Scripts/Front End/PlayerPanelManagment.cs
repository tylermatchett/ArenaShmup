using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerPanelManagment : MonoBehaviour {

	public GameObject StartPanel;
	public GameObject CharacterPanel;
	public GameObject ReadyPanel;
	public GameObject ReadyManagerObject;

	ReadyManager readyManagerScript;
	public Image CharacterSelectImage;
	public Text PlayerTeam;
	public Text CharacterName;
	public Text CharacterWeapon;
	public Text CharacterAbility;

	public int CharacterID;

	public Player player;
	public enum PanelState {
		PressStart,
		CharacterSelect,
		Ready
	}
	public PanelState state = PanelState.PressStart;

	void Start () {
		DisablePanels();
		StartPanel.SetActive(true);
		readyManagerScript = ReadyManagerObject.GetComponent<ReadyManager>();
	}

	void Update () {
		// Change the character name, weapon, and ability based on the players selected character
		if (player != null) {
			CharacterName.text = player.character.Name;

			// Handle Player Input
			ManagePlayerInput();

			if (state == PanelState.CharacterSelect) {
				// Update Player UI with Choices
				UpdatePlayerPanel();
			}
		}
	}

	private void ManagePlayerInput() {
		if (state == PanelState.CharacterSelect) {
			if (player.device.Direction.Left.WasPressed) {
				//Debug.Log("Left was pressed");
				// Change character
				CharacterID--;
				if (CharacterID < 0) {
					CharacterID = GameManager.Instance.characterList.Count-1;
				}
			}

			if (player.device.Direction.Right.WasPressed) {
				//Debug.Log("Right was pressed");
				// Change character
				CharacterID++;
				if (CharacterID >= GameManager.Instance.characterList.Count) {
					CharacterID = 0;
				}
			}

			if (player.device.Action4.WasPressed) {
				// Change character
				player.TeamID++;
				if (player.TeamID > 3) {
					player.TeamID = 0;
				}
			}

			// On Ready, Set the character in the player based on the CharacterID
			if (player.device.Action1.WasPressed) {
				player.Ready = true;
				state = PanelState.Ready;
				readyManagerScript.ReadyCheck();
				player.character = GameManager.Instance.characterList[CharacterID];
				StateUpdate();
			}
		}

		if (player.device.Action2.WasPressed) {
			if (player.Ready) {
				player.Ready = false;
				state = PanelState.CharacterSelect;
				readyManagerScript.ReadyCheck();

				StateUpdate();
			} else {
				GameManager.Instance.RemovePlayerFromList(player);
				state = PanelState.PressStart;
				CharacterSelectImage.color = new Color(1f, 1f, 1f, 0f);
				StateUpdate();
				player = null;
			}
		}
	}

	private void UpdatePlayerPanel() {
		CharacterName.text = GameManager.Instance.characterList[CharacterID].Name;
		CharacterWeapon.text = GameManager.Instance.characterList[CharacterID].primaryWeapon;
		CharacterAbility.text = GameManager.Instance.characterList[CharacterID].Ability;
		CharacterSelectImage.sprite = GameManager.Instance.characterList[CharacterID].displayPortrait;
		CharacterSelectImage.color = new Color(1f, 1f, 1f, 1f);

		switch (player.TeamID) {
		case 0:
			PlayerTeam.text = "Team 1";
			break;
		case 1:
			PlayerTeam.text = "Team 2";
			break;
		case 2:
			PlayerTeam.text = "Team 3";
			break;
		case 3:
			PlayerTeam.text = "Team 4";
			break;
		}
	}

	protected void DisablePanels() {
		StartPanel.SetActive(false);
		CharacterPanel.SetActive(false);
		ReadyPanel.SetActive(false);
	}

	public void StateUpdate() {
		// Disable the panels held within
		DisablePanels();

		// Check the states and enable the correct panels
		switch (state) {
		case (PanelState.PressStart):
			StartPanel.SetActive(true);
			break;
		case (PanelState.CharacterSelect):
			CharacterPanel.SetActive(true);
			break;
		case (PanelState.Ready):
			ReadyPanel.SetActive(true);
			break;
		}
	}
}
