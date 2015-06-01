using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CameraTracking : MonoBehaviour {
	// default values for the camera restrictions
	public float MinScreenZoom = 15f;
	public float MaxScreenZoom = 50f;
	public float ScreenAdjustmentSpeed = 1.0f;
	public float VerticalScreenAdjustmentSpeed = 7f;

	// Level Information
	//public GameObject LevelManangerObject;
	//LevelManager levelManager;
	Vector3 levelMiddlePoint = Vector3.zero;

	// Player Information
	List<GameObject> playerList = new List<GameObject>();
	List<Vector3> targets = new List<Vector3>();

	// Camera Variables
	Vector3 NewCameraPosition;
	Vector3 MidPoint;
	float DistanceBetweenPlayers;
	float tempDistance;
	float CameraDistance = 10f;
	float aspectRatio;

	Vector3 subcamPos;

	void Start () {
		/*if (LevelManangerObject) {
			levelManager = LevelManangerObject.GetComponent<LevelManager>();
		}*/
		// Get the map center point
		// TODO Get This Implemented
		//levelMiddlePoint = levelManager.level.CenterOfMap;

		// get the aspect ratio
		aspectRatio = Screen.width / Screen.height;
	}

	void LateUpdate() {
		GetTargets();
		CalculateMidPoint();
		
		// Zoom Level
		DistanceBetweenPlayers = GetLargestDistance() / 2.0f / aspectRatio;
		if (DistanceBetweenPlayers < MinScreenZoom) {
			DistanceBetweenPlayers = MinScreenZoom;
		} else if (DistanceBetweenPlayers > MaxScreenZoom) {
			DistanceBetweenPlayers = MaxScreenZoom;
		}
		CameraDistance = Mathf.Lerp(CameraDistance, DistanceBetweenPlayers, Time.deltaTime * ScreenAdjustmentSpeed);
		
		//NewCameraPosition = MidPoint;
		NewCameraPosition.x = Mathf.Lerp(transform.position.x, MidPoint.x, Time.deltaTime * VerticalScreenAdjustmentSpeed);
		NewCameraPosition.y = Mathf.Lerp(transform.position.y, MidPoint.y, Time.deltaTime * VerticalScreenAdjustmentSpeed);
		NewCameraPosition.z = (-8f - CameraDistance); // Because of Ortho

		Camera.main.orthographicSize = CameraDistance;
		transform.position = NewCameraPosition;
	}

	void CalculateMidPoint() {
		MidPoint = Vector3.zero;

		// Loop through the positions and get the sum
		foreach (Vector3 pos in targets) {
			MidPoint += pos;
		}

		// get the average between the points / Mid Point
		MidPoint /= targets.Count; // check this number

		if (targets.Count <= 0) {
			MidPoint = Vector3.zero;
		}
	}

	float GetLargestDistance() {
		float currentDistance = 0f;
		float largestDistance = 0f;

		for (int i = 0; i < targets.Count; i++) {
			for (int j = 0; j < targets.Count; j++) {
				currentDistance = Vector3.Distance(targets[i], targets[j]);

				if (currentDistance > largestDistance) {
					largestDistance = currentDistance;
				}
			}
		}

		return largestDistance;
	}

	void GetTargets() {
		playerList = GameObject.FindGameObjectsWithTag("Player").ToList();

		targets.Clear();

		foreach (GameObject go in playerList) {
			targets.Add(go.transform.position);
			
			// The more times I add the level point the more weight it carries
			// add 1 per player
			targets.Add(levelMiddlePoint);
		}
	}
}
