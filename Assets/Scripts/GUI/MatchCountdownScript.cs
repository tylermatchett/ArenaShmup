using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatchCountdownScript : MonoBehaviour {

	public GameObject matchManagerObject;
	public MatchManager matchManager;
	public bool Activated = true;

	public Text timerDisplay;
	Vector2 textOrigin;
	float tempDistance;

	float distanceOfPan = 100f;

	public float timerDefault;
	float timer;

	void Start () {
		if (matchManagerObject) {
			matchManager = matchManagerObject.GetComponent<MatchManager> ();
		}

		timerDisplay.text = "";
		textOrigin = timerDisplay.rectTransform.anchoredPosition;

		timer = timerDefault;
	}

	void Update () {
		if (Activated) {
			if (timer > 4f) {
				timerDisplay.text = "";
			} else if (timer > 1f) {
				timerDisplay.text = Mathf.FloorToInt(timer).ToString ();
			} else {
				timerDisplay.text = "Fight!";
			}

			if (timer <= 0f) {
				DestroyMatchCounter ();
			}

			tempDistance = timer % 1f;
			timerDisplay.rectTransform.anchoredPosition = textOrigin + new Vector2(distanceOfPan * tempDistance*tempDistance*tempDistance*tempDistance, 0f);

			timer -= Time.deltaTime;
		} else {
			timerDisplay.text = "";
		}
	}

	void DestroyMatchCounter() {
		matchManager.roundPhase++;
		matchManager.SignalRoundStart();
		matchManager.countdownActive = false;
		Activated = false;
		timer = timerDefault;
		//Activated = false;
		//timerDisplay.gameObject.SetActive(false);
		GameObject[] listOfPlayers = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject playerObj in listOfPlayers) {
			playerObj.SendMessage("RoundStarting", SendMessageOptions.DontRequireReceiver);
		}
	}
}
