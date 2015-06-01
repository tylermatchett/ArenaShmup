using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PulseText : MonoBehaviour {
	Text pressStart;
	Color tempTrans;
	bool reduceTrans = true;
	float minTrans = 0.05f;
	float maxTrans = 0.95f;
	public float pulseSpeed = 1f;

	void Start () {
		pressStart = GetComponent<Text>();
	}

	void Update () {
		tempTrans = pressStart.color;

		if (tempTrans.a < minTrans) {
			reduceTrans = false;
		} else if (tempTrans.a > maxTrans) {
			reduceTrans = true;
		}

		if (reduceTrans) {
			tempTrans.a -= pulseSpeed * Time.deltaTime;
		} else {
			tempTrans.a += pulseSpeed * Time.deltaTime;
		}

		pressStart.color = tempTrans;
	}
}
