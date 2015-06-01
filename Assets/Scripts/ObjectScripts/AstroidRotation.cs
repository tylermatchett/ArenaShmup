using UnityEngine;
using System.Collections;

public class AstroidRotation : MonoBehaviour {

	float rotateSpeed;
	void Start() {
		rotateSpeed = Random.Range(-5f, 5f);
		gameObject.transform.Rotate(new Vector3 (0f, 0f, 1f), Random.Range(-90f, 90f));
	}

	void Update () {
		gameObject.transform.Rotate(new Vector3 (0f, 0f, 1f), rotateSpeed * Time.deltaTime);
	}
}
