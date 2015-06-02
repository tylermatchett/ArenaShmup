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
		if (onDestroyPrefab != null) {
			GameObject Explosion = (GameObject)Instantiate(onDestroyPrefab, transform.position, transform.rotation);
			Explosion.GetComponent<projectile_stats>().damage = GetComponent<projectile_stats>().damage;
			Explosion.GetComponent<projectile_stats>().playerReference = GetComponent<projectile_stats>().playerReference;
			Camera.main.GetComponent<ScreenShake>().Shake(5f, 0.05f);
		}

		gameObject.SetActive(false);
	}
}