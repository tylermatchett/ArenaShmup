using UnityEngine;
using System.Collections;

public class projectile_destroy : MonoBehaviour {

	public GameObject onhitPrefab; // when it hits an enemy
	public GameObject onDestroyPrefab; // when it dies in space

	public float lifetime;
	public bool destroyOutOfBounds;

	float timer = 0f;

	void OnEnable() {
		timer = 0f;
	}

	void Update () {
		timer += Time.deltaTime;
		if (timer > lifetime) {
			Despawn();
		}
	}

	public void Despawn() {
		gameObject.SetActive(false);
	}
}