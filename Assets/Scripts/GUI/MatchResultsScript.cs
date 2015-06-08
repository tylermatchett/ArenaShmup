using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;

public class MatchResultsScript : MonoBehaviour {
	public GameObject MatchCountdown;
	public GameObject CountdownText;

    public GameObject resultsPanel;
    public GameObject ScreenDarkener;
	public Text teamText;
	public Image teamBackground;
	public List<Text> playerNameList = new List<Text>();
	public List<Text> playerStatsList = new List<Text>();

	public GameObject eomText;
	bool EndOfMatch = false;

	public void Start() {
		foreach (Text t in playerNameList) {
			t.text = "";
		}
		foreach (Text t in playerStatsList) {
			t.text = "";
		}
	}

	public void Update() {
		if (EndOfMatch) {
			InputDevice device = InputManager.ActiveDevice;
		
			if (device.Action1.WasReleased) {
				GameManager.Instance.ResetControllerLocks();
				GameManager.Instance.LoadState ("main_menu");
			}
		}
	}

	public void initResults(List<GameObject> players, int winningTeamID) {
		Invoke("SetMatchEnd", 2f);

		MatchCountdown.SetActive(false);
		CountdownText.SetActive(false);

        ScreenDarkener.SetActive(true);

		for (int i = 0; i < players.Count; i++) {
			playerNameList[i].text = players[i].GetComponent<PlayerManager>().player.character.Name;
			playerStatsList[i].text = "Kills " + players[i].GetComponent<PlayerManager>().playerMatchStats.kills + " / Deaths " +  players[i].GetComponent<PlayerManager>().playerMatchStats.deaths;
		}

		teamText.text = "Team " + (winningTeamID+1);

		Color c = new Color();
		switch (winningTeamID) {
		case 0:
			c = new Color(0f, 0f, 1f, 0.4f);
			break;
		case 1:
			c = new Color(1f, 0f, 0f, 0.4f);
			break;
		case 2:
			c = new Color(0f, 1f, 0f, 0.4f);
			break;
		case 3:
			c = new Color(0f, 0f, 0f, 0.4f);
			break;
		default:
			break;
		}
		teamBackground.color = c;

		resultsPanel.SetActive(true);
	}

	void SetMatchEnd() {
		eomText.SetActive(true);
		EndOfMatch = true;
	}
}
