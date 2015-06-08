using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;

public class MatchManager : MonoBehaviour {
	// Elimination Game Mode (TODO Rename this Match Manager to Elimination Match Manager)
	// Keeps track of the score and when to end games
	// Handles the spawning and respawning for the match

	// GO Vars
	//public GameObject levelManagerObject;
	//LevelManager levelManager;
	public GameObject MatchCountdownObject;
	public GameObject MatchResultsObject;

	// Player vars
	public GameObject playerPrefab;
	//List<Player> playerList = new List<Player>();
	List<GameObject> playerObjectList = new List<GameObject>();
	bool[] playerTeams = new bool[] {false, false, false, false};
	float spawnRadius = 3f;
	Vector2[] playerSpawnOffset;
	List<Vector2> teamSpawnLocation;
	int teamCount = 0;

	public List<GameObject> playerUIPanels;

	// Match Vars
	public MatchStats MatchStatsTracker;

	// Round vars
	public int roundCount = 1;
	public bool debug_getMatchStats = false;
	public bool countdownActive = false;

	// Spawn Vars
	public List<Vector2> spawnPoints;
	public float SpawnPointRadius = 1.5f;

	public enum RoundPhase {
		StartRound,
		InSession,
		EndRound
	}
	public RoundPhase roundPhase;

	public enum MatchPhase {
		StartMatch,	// Setup the match, init the stat tracker for the round
		InSession,	// Match in progress
		Paused,		// Pause menu
		EndMatch	// Match is over, goto victory scene
	}
	public MatchPhase matchPhase;
    MatchPhase MatchStateBeforePause;

	[Header ("Player UI References")]
	public List<GameObject> playerPanelList = new List<GameObject>();
	public List<Text> playerNameList = new List<Text>();
	public List<Image> playerPortraitList = new List<Image>();
	public List<Image> playerHPScaleList = new List<Image>();
	public List<Image> playerAmmoScaleList = new List<Image>();
	public List<Text> playerAmmoRemainingList = new List<Text>();
	public GameObject[] characterTextList;
    public GameObject[] sfx_smallExplosionList;
    public GameObject MatchRoundResultsObject;
    public GameObject MatchIntroObject;
    public GameObject PauseMenu;

    bool initMatchFinished = false;
	bool delayedRoundEnd = false;

    int finalTeamRemaining;

	void Start () {
		matchPhase = MatchPhase.StartMatch;
		roundPhase = RoundPhase.StartRound;

		characterTextList = GameObject.FindGameObjectsWithTag ("deathText");
        sfx_smallExplosionList = GameObject.FindGameObjectsWithTag("sfx_SmallExplosion");

        foreach (GameObject go in characterTextList) {
            go.SetActive(false);
        }
        foreach (GameObject go in sfx_smallExplosionList) {
            go.SetActive(false);
        }

		playerSpawnOffset = new Vector2[] {new Vector3(spawnRadius, 0f, 0f), new Vector3(0f, -spawnRadius, 0f), new Vector3(0f, spawnRadius, 0f), new Vector3(-spawnRadius, 0f, 0f)};
	}

    public GameObject GetSmallExplosionSFXInstance() {
        GameObject temp = null;
        bool foundOne = false;
        for (int i = 0; i < sfx_smallExplosionList.Length; i++) {
            if (!sfx_smallExplosionList[i].activeSelf && !foundOne) {
                temp = sfx_smallExplosionList[i];
                foundOne = true;
            }
        }
        return temp;
    }

	void Update () {
        InputDevice device = InputManager.ActiveDevice;

        if (device.MenuWasPressed) {
            if (!MatchIntroObject.activeSelf) {
                PauseMenu.SetActive(true);
                // Pause the players
                GameObject[] tempPlayerList = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject g in tempPlayerList) {
                    if (g.GetComponent<PlayerManager>().playerState == PlayerManager.PlayerState.Alive) {
                        g.GetComponent<PlayerManager>().playerState = PlayerManager.PlayerState.Paused;
                    }
                }
                MatchStateBeforePause = matchPhase;
                matchPhase = MatchPhase.Paused;
            }
        }

		switch (matchPhase) {
		case MatchPhase.StartMatch:
            if (!initMatchFinished) {
                // Create/Restart the stat tracker
                CreateStatManager();

                // Get the level spawn locations
                GetLevelSpawnLocations();

                // Create the player objects
                CreatePlayers();

                initMatchFinished = true;
            }
            // Create the Map introduction here
			//matchPhase = MatchPhase.InSession;
			break;
		case MatchPhase.InSession:
			SimulateRounds();
			//CheckForPlayerPause();
			break;
		case MatchPhase.Paused:
			//Debug.Log("Game Paused");
			GamePaused();
			break;
		case MatchPhase.EndMatch:
			//Debug.Log("End of Match reached");
			EndMatch();
			break;
		}

		if (debug_getMatchStats) {
			debug_getMatchStats = !debug_getMatchStats;
			MatchStatsTracker.GetDebugReport();
		}

        if (matchPhase != MatchPhase.Paused) {
		    if (GameManager.Instance.slowTime) {
			    Time.timeScale = GameManager.Instance.timeScale;
		    } else {
			    Time.timeScale = 1f;
		    }
        }
	}

	// Round update
	void SimulateRounds() {
		// Only accept player input on characters in the round phase
		switch (roundPhase) {
		case RoundPhase.StartRound:
			StartRound();
			break;
		case RoundPhase.InSession:
			// Round in session let the players act
			if (IsRoundDone()) {
				//Debug.Log("Changing to End Round");
				roundPhase = RoundPhase.EndRound;
			}
			break;
		case RoundPhase.EndRound:
			EndRound();
			break;
		}
	}

	// Match Methods
	void GamePaused() {
		// Show the pause menu
		// If unpaused action has been sent go to InSession
	}

	void EndMatch() {
		// The Match is done and the goal has been reached
		// Store the match rankings and stats in the game manager
		// change the scene to the end game scene
	}

    void ShowEndMatchStats() {
        MatchResultsObject.GetComponent<MatchResultsScript>().initResults(playerObjectList, finalTeamRemaining);
    }


	void CreateStatManager() {
		// create the stat manager here, holds all of the players and their teams current score towards the game end objective
		MatchStatsTracker = new MatchStats();

		roundCount = 1;
	}

	void GetLevelSpawnLocations() {
		// Get the level spawn locations from the levelManager
		//spawnPoints = levelManager.spawnPoints;
		//Debug.LogError("Need Level Manager Spawn Points");
		teamSpawnLocation = new List<Vector2>();
	}

	void CreatePlayers() {
		teamCount = 0;

		// Get the player list from the gameManager
		for (int i = 0; i < GameManager.Instance.playerList.Length; i++) {
			// Make sure there is a player to add to the list
			if (GameManager.Instance.playerList[i] != null) {
				// Add the player data to the player list
				//playerList.Add(GameManager.Instance.playerList[i]); // do we even need the player data list? probably for quick team checks without needing to lookup 

				// Create a player prefab and add the player to the prefab script
				playerObjectList.Add((GameObject) Instantiate(playerPrefab));
				playerObjectList[playerObjectList.Count-1].GetComponent<PlayerManager>().player = GameManager.Instance.playerList[i];
				playerObjectList[playerObjectList.Count-1].GetComponent<PlayerManager>().SetCharacterText(characterTextList[playerObjectList.Count-1]);
				playerObjectList[playerObjectList.Count-1].GetComponent<PlayerManager>().CreatePlayerCharacter();
				playerObjectList[playerObjectList.Count-1].GetComponent<PlayerManager>().CreatePlayerUI(playerPanelList[i],playerNameList[i], playerHPScaleList[i], playerAmmoScaleList[i], playerAmmoRemainingList[i]);
				playerObjectList[playerObjectList.Count-1].SendMessage("MatchPaused", SendMessageOptions.DontRequireReceiver);
				playerObjectList[playerObjectList.Count-1].name = "Player " + GameManager.Instance.playerList[i].PlayerNumber;
				playerObjectList[playerObjectList.Count-1].SetActive(false);
				playerTeams[GameManager.Instance.playerList[i].TeamID] = true;
			}
		}

		for (int i = 0; i < playerTeams.Length; i++) {
			if (playerTeams[i]) {
				// Count how many teams are actively used
				teamCount++;
			}
		}

		// Designate the spawn locations based on # of teams being used
		for (int i = 0; i < teamCount; i++) {
			// This requires us to add the spawn points in the order that we need to use them
			teamSpawnLocation.Add(spawnPoints[i]);
		}

		//Debug.Log("End of player create");
	}


	// Round Methods
	void StartRound() {
		// Initialize the round
		if (!countdownActive) {
			Debug.Log("Start Round - Round " + roundCount + " Started");
			SpawnPlayers();
			countdownActive = true;
		}

		// set an instance to the matchmanager and set the roundphase during its destruction
		MatchCountdownObject.GetComponent<MatchCountdownScript>().Activated = true;
	}

	void SpawnPlayers() {
		// loop through the team array
		//	if the team is being used loop through the players and spawn all players on that team on the designated spawn location for that team
		// Loop through the team array
		int teamSpawnPointUsedCount = 0;
		for (int i = 0; i < playerTeams.Length; i++) {
			if (playerTeams[i]) {
				// This teamID is used
				// Loop through the players
				int playersOnThisTeamCount = 0;
				foreach (GameObject playerObj in playerObjectList) {
					if (playerObj.GetComponent<PlayerManager>().player.TeamID == i) {
						// If the player is a member of this team, spawn them at this position with the required offset
						playerObj.GetComponent<PlayerManager>().Spawn(teamSpawnLocation[teamSpawnPointUsedCount] + playerSpawnOffset[playersOnThisTeamCount]);
						playersOnThisTeamCount++;
					}
				}

				teamSpawnPointUsedCount++;
			}
		}
	}

	bool IsRoundDone() {
		// call this from player death in playerManager and change the return var to set a bool in this script
		bool[] roundTeamCheck = new bool[] {false, false, false, false};
		int tempTeamCount = 0;
		int lastTeamRemaining = -1;

		foreach (GameObject playerObject in playerObjectList) {
			if (playerObject.GetComponent<PlayerManager>().playerState == PlayerManager.PlayerState.Alive) {
				roundTeamCheck[playerObject.GetComponent<PlayerManager>().player.TeamID] = true;
				lastTeamRemaining = playerObject.GetComponent<PlayerManager>().player.TeamID;
			}
		}

		foreach (bool b in roundTeamCheck) {
			if (b == true) {
				tempTeamCount++;
			}
		}

		if (tempTeamCount < 2) {
			MatchStatsTracker.setTeamRound(lastTeamRemaining);

			if (MatchStatsTracker.TeamWon(lastTeamRemaining)) {
				// There was a winner!
				// activate the match results
                // feed the match results the data
                finalTeamRemaining = lastTeamRemaining;
                Invoke("ShowEndMatchStats", 2f);

				// Change the state of the match
				matchPhase = MatchPhase.EndMatch;
			}
			return true;
		}
		return false;
	}

	void EndRound() {
		if (!delayedRoundEnd) {
			Invoke ("DelayStartOfRound", 2f);
			delayedRoundEnd = true;
		}
	}

	void DelayStartOfRound() {
        // Show end of round display and on any button press goto next round, turn off controls for that
        if (matchPhase != MatchPhase.EndMatch) {
            MatchRoundResultsObject.SetActive(true);
            MatchRoundResultsObject.GetComponent<MatchRoundResultsScript>().initRoundTotals(playerObjectList);
        }
	}

    public void EndingRoundAfterDelay() {
        roundCount++;

        foreach (GameObject activeProjectile in GameObject.FindGameObjectsWithTag("Projectile")) {
            activeProjectile.SetActive(false);
        }

        roundPhase = RoundPhase.StartRound;

        delayedRoundEnd = false;
    }

	public void SignalRoundStart() {
		foreach (GameObject playerObj in playerObjectList) {
			playerObj.SendMessage("MatchStarting", SendMessageOptions.DontRequireReceiver);
		}
	}

    public void UnPauseMatch() {
        matchPhase = MatchStateBeforePause;
    }
}
