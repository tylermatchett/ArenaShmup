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

	bool alternateForm = false;

	public Player player;
	public enum PanelState {
		PressStart,
		CharacterSelect,
		Ready
	}
	public PanelState state = PanelState.PressStart;

    PlayMenuSounds menuSounds;

    void Start() {
        menuSounds = GameObject.FindGameObjectWithTag("fe_sfx").GetComponent<PlayMenuSounds>();
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
                menuSounds.PlaySwitchSFX();
				//Debug.Log("Left was pressed");
				// Change character
				CharacterID--;
				if (CharacterID < 0) {
					CharacterID = GameManager.Instance.characterList.Count-1;
				}
			}

            if (player.device.Direction.Right.WasPressed) {
                menuSounds.PlaySwitchSFX();
				//Debug.Log("Right was pressed");
				// Change character
				CharacterID++;
				if (CharacterID >= GameManager.Instance.characterList.Count) {
					CharacterID = 0;
				}
			}

            if (player.device.LeftTrigger.WasPressed) {
                menuSounds.PlaySwitchSFX();
				// Match Type
				if (GameManager.Instance.matchType == GameManager.MatchType.Kills) {
					GameManager.Instance.matchType = GameManager.MatchType.Rounds;
				} else {
					GameManager.Instance.matchType = GameManager.MatchType.Kills;
				}
			}

            if (player.device.RightTrigger.WasPressed) {
                menuSounds.PlaySwitchSFX();
				// Match Goal
				// Kills & Rounds: Ft3, Ft5, Ft7, Ft9, Ft10, Ft15, Ft20
				GameManager.Instance.matchGoalCounter++;
				if (GameManager.Instance.matchGoalCounter >= GameManager.Instance.matchGoals.Count) {
					GameManager.Instance.matchGoalCounter = 0;
				}
				GameManager.Instance.matchGoal = GameManager.Instance.matchGoals[GameManager.Instance.matchGoalCounter];
			}

			if (player.device.Action3.WasPressed) {
				// Change character to alt
				alternateForm = !alternateForm;
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
                if (!GameManager.Instance.characterList_Lock[CharacterID]) {
                    menuSounds.PlaySelectSFX();
					player.Ready = true;
					state = PanelState.Ready;
					readyManagerScript.ReadyCheck();
					if (!alternateForm) {
						player.character = GameManager.Instance.characterList[CharacterID];
					} else {
						player.character = GameManager.Instance.characterList_alt[CharacterID];
					}
					GameManager.Instance.characterList_Lock[CharacterID] = true;
					GameManager.Instance.playerList[player.PlayerNumber].character = player.character;
					StateUpdate();
				}
			}
		}

        if (player.device.Action2.WasPressed) {
            menuSounds.PlayBackSFX();
			if (player.Ready) {
				player.Ready = false;
				state = PanelState.CharacterSelect;
				
				GameManager.Instance.characterList_Lock[CharacterID] = false;
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
		if (!alternateForm) {
			CharacterName.text = GameManager.Instance.characterList [CharacterID].Name;
			CharacterWeapon.text = GameManager.Instance.characterList [CharacterID].primaryWeapon;
			CharacterAbility.text = GameManager.Instance.characterList [CharacterID].Ability;
			CharacterSelectImage.sprite = GameManager.Instance.characterList [CharacterID].displayPortrait;
		} else {
			CharacterName.text = GameManager.Instance.characterList_alt [CharacterID].Name;
			CharacterWeapon.text = GameManager.Instance.characterList_alt [CharacterID].primaryWeapon;
			CharacterAbility.text = GameManager.Instance.characterList_alt [CharacterID].Ability;
			CharacterSelectImage.sprite = GameManager.Instance.characterList_alt [CharacterID].displayPortrait;
		}
		if (GameManager.Instance.characterList_Lock [CharacterID]) {
			CharacterSelectImage.color = new Color (0.5f, 0.5f, 0.5f, 1f);
		} else {
			CharacterSelectImage.color = new Color (1f, 1f, 1f, 1f);
		}

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
