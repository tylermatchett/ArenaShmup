using UnityEngine;
using System.Collections;

public class DestroyAfterSetTime : MonoBehaviour {

	public float timeToWait = 3f;

	void Start() {
		Destroy(gameObject,	timeToWait);
	}
}
