using UnityEngine;
using UnityEngine.UI;
using InControl;
using System.Collections;
using System.Collections.Generic;

public class MatchRoundResultsScript : MonoBehaviour {

    public List<Text> playerNameList = new List<Text>();
    public List<Text> playerStatsList = new List<Text>();

    float timer = 0f;
    float timerTotal = 0.5f;

    int playerCount;

    List<int> playerKills = new List<int>() { 0, 0, 0, 0 };
    List<int> playerKillsLastRound = new List<int>() { 0, 0, 0, 0 };
    List<int> playerKillsDifference = new List<int>() { 0, 0, 0, 0 };

    public Text RoundGoals;
    public GameObject continueInstructions;
    public GameObject endOfRoundUI;
    public GameObject ScreenDarkener;

    void OnEnable() {
        timer = 0f;
        endOfRoundUI.SetActive(true);
        for (int i = 0; i < playerNameList.Count; i++) {
            playerNameList[i].text = "";
            playerStatsList[i].text = "";
        }
        ScreenDarkener.SetActive(true);
        
        string roundMode = "";
        if (GameManager.Instance.matchType == GameManager.MatchType.Kills) {
            roundMode = "Kills";
        }
        else {
            roundMode = "Rounds";
        }

        RoundGoals.text = "First to " + GameManager.Instance.matchGoal + " " + roundMode;
    }

    void Update() {
        InputDevice device = InputManager.ActiveDevice;

        // store last stats and have the new stats climb when shown
        timer += Time.deltaTime;
        if (timer > timerTotal) {
            timer = timerTotal;
            continueInstructions.SetActive(true);

            // Show the a to continue

            if (device.Action1.WasReleased) {
                // Continue Rounds
                continueInstructions.SetActive(false);
                endOfRoundUI.SetActive(false);
                ScreenDarkener.SetActive(false);

                for (int i = 0; i < playerCount; i++) {
                    playerKillsLastRound[i] = playerKills[i];
                }

                GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>().EndingRoundAfterDelay();
                gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < playerCount; i++) {
            playerStatsList[i].text = (Mathf.FloorToInt((float)playerKillsLastRound[i] + (float) ((float)playerKillsDifference[i] * (timer / timerTotal)))).ToString() + " Kills";
        }
	}

    public void initRoundTotals(List<GameObject> players) {
        playerCount = players.Count;

        for (int i = 0; i < players.Count; i++) {
            playerNameList[i].text = players[i].GetComponent<PlayerManager>().player.character.Name;
            playerKills[i] = players[i].GetComponent<PlayerManager>().playerMatchStats.kills;
            playerKillsDifference[i] = playerKills[i] - playerKillsLastRound[i];
        }

        endOfRoundUI.SetActive(true);
    }
}
