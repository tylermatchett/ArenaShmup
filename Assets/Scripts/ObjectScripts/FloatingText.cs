using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

	public float floatSpeed = 0.5f;
	public Vector3 floatDirection = new Vector3(0f, 1f, 0f);

	void Update () {
		transform.position += floatDirection * floatSpeed * Time.deltaTime;
	}
}
