using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public Player player;
	MatchManager matchManager;
	public PlayerMatchStats playerMatchStats;
	ScreenShake shakeCamera;

	public enum PlayerState {
		Alive,
		Dead,
		Paused
	}
	public PlayerState playerState = PlayerState.Paused;

	PlayerStats playerStats;
	public GameObject DeathExplosionFX;
	public GameObject lastPlayerToHitMe;

	public GameObject textObj = null;
	public Text wordsText = null;
	public enum WordsToSay {
		Hit,
		Kill,
		Victory,
		Death
	}

	public void Start() {
		matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
		shakeCamera = Camera.main.GetComponent<ScreenShake>();
	}

	public void CreatePlayerCharacter() {
		GameObject tempCharacterObj = (GameObject) Instantiate(player.character.characterPrefab);
		tempCharacterObj.transform.parent = gameObject.transform;
		tempCharacterObj.transform.position = Vector3.zero;
		tempCharacterObj.BroadcastMessage("SetPlayerObj", gameObject, SendMessageOptions.DontRequireReceiver);
		playerStats = GetComponent<PlayerStats>();
		playerMatchStats = new PlayerMatchStats();
	}

	public void CreatePlayerUI(GameObject panel, Text plName, Image hpScale, Image ammoScaleAndReloadTimer, Text AmmoRemaining) {
		plName.text = player.character.Name;

		// Send the health and ammo vars to the managers for those
		gameObject.SendMessage("UIHealthControlsImage", hpScale, SendMessageOptions.DontRequireReceiver);
		gameObject.BroadcastMessage("UIWeaponControlsImage", ammoScaleAndReloadTimer, SendMessageOptions.DontRequireReceiver);
		gameObject.BroadcastMessage("UIWeaponControlsText", AmmoRemaining, SendMessageOptions.DontRequireReceiver);

		panel.SetActive(true);
	}

	public void Spawn(Vector3 location) {
		playerState = PlayerState.Paused;

		// Reset Health and Ammo Stats
		ResetStats();

		transform.position = location;

		gameObject.SetActive(true);
	}

	public void SetCharacterText(GameObject go) {
		textObj = go;
		wordsText = go.GetComponent<Text>();
	}

	public void Despawn() {
		playerState = PlayerState.Dead;
		gameObject.BroadcastMessage("EndFire", SendMessageOptions.DontRequireReceiver);
		gameObject.SetActive(false);
	}

	void ResetStats() {
		// Set the players health to full
		playerStats.ResetStats();

		// Reset the ability ticker/levels/charge?
		// Reset the ability use counter if it exists
		// Reload the players Weapons
		gameObject.BroadcastMessage("ResetWeapon", SendMessageOptions.DontRequireReceiver);

		// Set the players velocity to 0
		gameObject.SendMessage("ResetMovement", SendMessageOptions.DontRequireReceiver);
	}

	void OnEnable() {
		
	}
	
	void OnDisable() {
		CancelInvoke();
	}

	public void TakeDamage(float dmg) {
		playerStats.TakeDamage(dmg);
		if (playerStats.CheckDeath ()) {
			shakeCamera.Shake(30f, 1.0f);
			SlowMoForDeath ();
			Invoke ("Die", 0.05f);
		} else {
			if (Random.Range (0f, 1f) > 0.80f) {
				Invoke("HitWords", 0.25f);
			}
			shakeCamera.Shake(5f, 0.1f);
			SlowMoOnHit();
		}
	}
	
	void SlowMoOnHit() {
		if (!GameManager.Instance.slowTime) {
			GameManager.Instance.timeScale = 0.7f;
			GameManager.Instance.slowTime = true;
			GameManager.Instance.ReturnTimeToNormal (0.05f);
		}
	}
	
	void SlowMoForDeath() {
		GameManager.Instance.timeScale = 0.25f;
		GameManager.Instance.slowTime = true;
		GameManager.Instance.ReturnTimeToNormal(0.25f);
	}

	void Die() {
		// Die
		// Spawn permanence stuff like ship parts and send them flying
		// Spawn explosion
		Instantiate(DeathExplosionFX, gameObject.transform.position, gameObject.transform.rotation);
		// Have the character cry out in death
		// Record Death for the team and player, and record the kill for the enemy player and enemy team
		playerMatchStats.deaths++;
		lastPlayerToHitMe.GetComponent<PlayerManager>().playerMatchStats.kills++;
		matchManager.MatchStatsTracker.setTeamDeath(player.TeamID);
		matchManager.MatchStatsTracker.setTeamKill(lastPlayerToHitMe.GetComponent<PlayerManager>().player.TeamID);

		if (Random.Range (0f, 1f) > 0.25f) {
			SayWords(WordsToSay.Death);
		} else {
			lastPlayerToHitMe.GetComponent<PlayerManager>().SayWords(WordsToSay.Kill);
		}

		Despawn();
	}

	void HitWords() {
		SayWords(WordsToSay.Hit);
	}

	void SayWords(WordsToSay wordSwitch) {
		// Get death text
		if (!textObj.activeSelf) {
			string whatCharacterWillSay = "";

			switch (wordSwitch) {
			case WordsToSay.Hit:
				whatCharacterWillSay = "\"" + player.character.GetGettingHitText() + "\"";
				break;
			case WordsToSay.Kill:
				whatCharacterWillSay = "\"" + player.character.GetKillText() + "\"";
				break;
			case WordsToSay.Death:
				whatCharacterWillSay = "\"" + player.character.GetDeathText() + "\"";
				break;
			}
		
			wordsText.text = whatCharacterWillSay;

			textObj.transform.position = transform.position;
			textObj.SetActive (true);
		}
	}

	public void RoundStarting() {
		playerState = PlayerState.Alive;
		gameObject.BroadcastMessage("ResetWeapon", SendMessageOptions.DontRequireReceiver);
	}
}
