using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayRoundTypeAndGoal : MonoBehaviour {

	public Text txtMatchType;
	public Text txtMatchGoal;

	void Start () {
	
	}

	void Update () {
		if (GameManager.Instance.matchType == GameManager.MatchType.Kills) {
			txtMatchType.text = "Match Type - Kills";
		} else {
			txtMatchType.text = "Match Type - Rounds";
		}
		txtMatchGoal.text = "Match Goal - First to " + GameManager.Instance.matchGoal.ToString();
	}
}
