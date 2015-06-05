using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {
	[Header ("Character Basic Variables")]
	public string Name;
	public string primaryWeapon;
	public string Ability;
	public GameObject characterPrefab;
	public Sprite displayPortrait;
    public bool unlocked = false;

	[Header ("Character Sayings")]
	public List<string> roundWinWords = new List<string>();
	public List<string> gettingHitWords = new List<string>();
	public List<string> killingHitWords = new List<string>();
	public List<string> deathWords = new List<string>();
	
	public string GetRoundWinText() {
		if (roundWinWords.Count > 0) {
			return roundWinWords[Random.Range(0, roundWinWords.Count)];
		}

		return "";
	}
	public string GetGettingHitText() {
		if (gettingHitWords.Count > 0) {
			return gettingHitWords[Random.Range(0, gettingHitWords.Count)];
		}
		
		return "";
	}
	public string GetKillText() {
		if (killingHitWords.Count > 0) {
			return killingHitWords[Random.Range(0, killingHitWords.Count)];
		}
		
		return "";
	}
	public string GetDeathText() {
		if (deathWords.Count > 0) {
			return deathWords[Random.Range(0, deathWords.Count)];
		}
		
		return "";
	}
}
