using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public Player[] playerList = new Player[4];
	public bool[] playerNumberList = new bool[] {false, false, false, false};
	public List<Character> characterList;
	public List<Character> characterList_alt;
	public List<bool> characterList_Lock;
	//public Level[] levelList = new Level[2];
	//public Level SelectedLevel;

	// Game Menu Sound Options
	[Header ("Volume Settings")]
	[Range (0f, 1f)]
	public float Volume_Master = 1.0f;
	[Range (0f, 1f)]
	public float Volume_MenuMusic = 1.0f;
	[Range (0f, 1f)]
	public float Volume_BattleMusic = 1.0f;
	[Range (0f, 1f)]
	public float Volume_SoundEffects = 1.0f;

	[Header ("Graphics Settings")]
	public int AntiAliasing = 8;
	public bool VSync = false;

	[Header ("Gameplay Settings")]
	public bool FriendlyFire = false;
	public float globalBulletSpeedModifier = 2f;
	public float globalSpeedModifier = 1.5f;

	public enum MatchType {
		Kills,
		Rounds
	}
	public List<int> matchGoals;
	public int matchGoalCounter = 1;

	[Header ("Match Settings")]
	public MatchType matchType = MatchType.Kills;
	public int matchGoal = 5;

	[Header ("Slow Time")]
	public bool slowTime = false;
	public float timeScale = 1f;

	static GameManager _instance;
	static public bool isActive { 
		get { 
			return _instance != null; 
		} 
	}

    public string ActiveState = "";

	static public GameManager Instance {
		get {
			if (_instance == null) {
				_instance = Object.FindObjectOfType(typeof(GameManager)) as GameManager;
					
				if (_instance == null) {
					GameObject go = new GameObject("GameManager");
					DontDestroyOnLoad(go);
					_instance = go.AddComponent<GameManager>();
					Debug.Log("DebugLog: Creating A New Game Manager");
				}

				_instance.InitializeGameVariables();
			}
			return _instance;
		}
	}

	public void OnApplicationQuit() {
		_instance = null;
	}

	public void LoadState(string state) {
        ActiveState = state;
		Application.LoadLevel(state);
	}

	public bool DeviceLocked(InputDevice d) {
		for (int i = 0; i < playerList.Length; i++) {
			if (playerList[i] != null) {
				if (int.Parse(playerList[i].device.Meta.Substring(playerList[i].device.Meta.IndexOf("[id: ") + "[id: ".Length, 1)) == int.Parse(d.Meta.Substring(d.Meta.IndexOf("[id: ") + "[id: ".Length, 1))) {
					//Debug.Log("Controller Already Set");
					return true;
				}
			}
		}
		return false;
	}

	public void ResetControllerLocks() {
		for (int i = 0; i < playerList.Length; i++) {
			playerList[i] = null;
			playerNumberList[i] = false;
		}
	}

	public void RemovePlayerFromList(Player player) {
		playerList [player.PlayerNumber] = null;
		playerNumberList[player.PlayerNumber] = false;
		player = null;
	}

	//public void LevelSelected(Level l) {
		//SelectedLevel = l;
	//}

	private void InitializeGameVariables() {
		// Init Characters
		characterList = new List<Character>();
		characterList.Add(((GameObject) Resources.Load("Prefabs/CharacterSelectors/cs_BlueLeader")).GetComponent<Character>());
		characterList.Add(((GameObject) Resources.Load("Prefabs/CharacterSelectors/cs_RedTwo")).GetComponent<Character>());
		characterList.Add(((GameObject) Resources.Load("Prefabs/CharacterSelectors/cs_TealAvenger")).GetComponent<Character>());
		characterList.Add(((GameObject) Resources.Load("Prefabs/CharacterSelectors/cs_YellowMarauder")).GetComponent<Character>());

		// init alt character list
		characterList_alt = new List<Character>();
		characterList_alt.Add(((GameObject) Resources.Load("Prefabs/CharacterSelectors/cs_BlueLeader_alt")).GetComponent<Character>());
		characterList_alt.Add(((GameObject) Resources.Load("Prefabs/CharacterSelectors/cs_RedTwo_alt")).GetComponent<Character>());
		characterList_alt.Add(((GameObject) Resources.Load("Prefabs/CharacterSelectors/cs_TealAvenger_alt")).GetComponent<Character>());
		characterList_alt.Add(((GameObject) Resources.Load("Prefabs/CharacterSelectors/cs_YellowMarauder_alt")).GetComponent<Character>());

		characterList_Lock = new List<bool>() {false, false, false, false};

		matchGoals = new List<int>() {3, 5, 7, 9, 10, 15, 20};
		// Init Levels
		//levelList [0] = new Level ("Dev Map", "level_dev_test", Vector2.zero);
		//levelList [1] = new Level ("Space Elevator", "level_space_elevator", Vector2.zero);
	}

	public void ReturnTimeToNormal(float f) {
		Invoke("TimeToNormal", f);
	}

	void TimeToNormal() {
		timeScale = 1f;
		slowTime = false;
	}
}