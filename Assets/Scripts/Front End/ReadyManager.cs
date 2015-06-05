using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviour {

	bool AllPlayersReady = false;
	float totalCountdown = 3.0f;
	float countdown;

	int readyCount = 0;
	int playerCount = 0;

	public GameObject goReadyText;
	public Text readyText;

	public string levelToLoad;

	void Start () {
		countdown = totalCountdown;
		readyText = goReadyText.GetComponent<Text>();
	}

	void Update() {
		ReadyCheck();

		if (AllPlayersReady) {
			if (countdown <= -0.25f) {
				// TODO Advance to level select
				// Temporary code
				GameManager.Instance.LoadState(levelToLoad);
			}
			countdown -= Time.deltaTime;
			if (countdown > 0f) {
				readyText.text = "Game starts in " + Mathf.CeilToInt(countdown).ToString();
			} else {
				readyText.text = "Go";
			}
		} else {
			readyText.text = "Press Start to Join";
		}
	}

	// Every time a player is ready and not ready
	public void ReadyCheck () {
		playerCount = 0;
		readyCount = 0;

		// If all players are ready set bool
		foreach (Player p in GameManager.Instance.playerList) {
			if (p != null) {
				playerCount++;
				if (p.Ready) {
					readyCount++;
				}
			}
		}

		if ((playerCount > 1) && (readyCount == playerCount)) {
			AllPlayersReady = true;
		} else {
			AllPlayersReady = false;

			ResetCountdown();
		}
	}

	void ResetCountdown() {
		countdown = totalCountdown;
	}
}
