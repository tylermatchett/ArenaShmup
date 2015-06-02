using UnityEngine;
using System.Collections;

public class DisableAfterSetTime : MonoBehaviour {

	public float timeToWait = 2f;
	
	void OnEnable() {
		Invoke ("DisableMe", timeToWait);
	}

	void DisableMe() {
		gameObject.SetActive(false);
	}
}
