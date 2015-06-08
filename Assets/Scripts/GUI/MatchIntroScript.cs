using UnityEngine;
using UnityEngine.UI;
using InControl;
using System.Collections;

public class MatchIntroScript : MonoBehaviour {

    public string MapName;
    public GameObject mapNameobj;
    public GameObject matchTypeAndGoalobj;
    public Text mapName;
    public Text matchTypeAndGoal;
    public GameObject pnlDarken;
    public GameObject matchCountDown;

    void Start() {
        mapName.text = MapName;
        string roundMode = "";
        if (GameManager.Instance.matchType == GameManager.MatchType.Kills) {
            roundMode = "Kills";
        } else {
            roundMode = "Rounds";
        }
        matchTypeAndGoal.text = "First to " + GameManager.Instance.matchGoal + " " + roundMode;
    }

	void Update () {
        InputDevice device = InputManager.ActiveDevice;

        if (device.AnyButton.WasPressed) {
            StartMatch();
        }
	}

    void StartMatch() {
        // Get the matchmanager and set the insession
        GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>().matchPhase = MatchManager.MatchPhase.InSession;
        //deactivate the objs

        mapNameobj.SetActive(false);
        matchTypeAndGoalobj.SetActive(false);
        pnlDarken.SetActive(false);
        matchCountDown.SetActive(true);
        gameObject.SetActive(false);
    }
}
