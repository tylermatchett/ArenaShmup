using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeTextOut : MonoBehaviour {
	
	public float timeToWait = 2f;
	float timer;
	Text thisText;
	Color originalColor;

	void Start() {
		thisText = GetComponent<Text>();
		originalColor = thisText.color;
		timer = timeToWait;
	}

	void OnEnable() {
		timer = timeToWait;
	}

	void Update () {
		timer -= Time.deltaTime;
		Color c = originalColor;
		if (timer <= 0f) {
			timer = 0f;
		}
		c.a = timer / timeToWait;
	}
}
