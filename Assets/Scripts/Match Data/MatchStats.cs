using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchStats {
	TeamStats[] teamStatList;

	public MatchStats() {
		teamStatList = new TeamStats[4] {new TeamStats(), new TeamStats(), new TeamStats(), new TeamStats()};
	}

	public void setTeamKill(int teamID) {
		teamStatList[teamID].kills++;
	}
	
	public void setTeamDeath(int teamID) {
		teamStatList[teamID].deaths++;
	}
	
	public void setTeamRound(int teamID) {
		teamStatList[teamID].rounds++;
	}

	public bool TeamWon(int teamID) {
		switch (GameManager.Instance.matchType) {
		case GameManager.MatchType.Rounds:
			if (teamStatList[teamID].rounds >= GameManager.Instance.matchGoal) {
				Debug.Log("Match Won by Team " + teamID + " Through Rounds.");
				return true;
			}
			break;
		case GameManager.MatchType.Kills:
			if (teamStatList[teamID].kills >= GameManager.Instance.matchGoal) {
				Debug.Log("Match Won by Team " + teamID + " Through Kills.");
				return true;
			}
			break;
		}

		return false;
	}

	public void GetDebugReport() {
		Debug.Log(" --- Team Stats --- ");
		for (int i = 0; i < teamStatList.Length; i++) {
			Debug.Log("Team " + (i+1) + " | Kills: " + teamStatList[i].kills + " | Deaths: " + teamStatList[i].deaths);
		}
		Debug.Log(" --- End Stats --- ");
	}
}

