using UnityEngine;
using System.Collections;

public class MatchCameraOrthoSize : MonoBehaviour {

	public Camera mainCam;
	Camera thisCam;

	void Start() {
		thisCam = GetComponent<Camera>();
	}

	void Update () {
		thisCam.orthographicSize = mainCam.orthographicSize;
	}
}
