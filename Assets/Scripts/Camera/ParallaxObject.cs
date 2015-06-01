using UnityEngine;
using System.Collections;

public class ParallaxObject : MonoBehaviour {
	
	public float parallaxSpeed = 0.5f;
	public float scale = 0.5f;

	Camera cam;

	Vector3 camLastPosition;
	public Vector3 offset;

	void Start () {
		cam = Camera.main;
		camLastPosition = cam.transform.position;
		gameObject.transform.localScale = new Vector3(scale, scale, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		offset = camLastPosition - cam.transform.position;
		offset.z = 0f;
		transform.position -= offset * parallaxSpeed;
		camLastPosition = cam.transform.position;
	}
}
