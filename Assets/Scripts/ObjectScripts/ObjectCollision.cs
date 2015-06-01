using UnityEngine;
using System.Collections;

public class ObjectCollision : MonoBehaviour {
	
	public GameObject bulletHitFX;
	ScreenShake shakeCamera;
	
	public void Start() {
		shakeCamera = Camera.main.GetComponent<ScreenShake>();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		gameObject.SendMessage("TakeDamage", collider.gameObject.transform.parent.GetComponent<projectile_stats>().damage, SendMessageOptions.DontRequireReceiver);

		// Flash the object white
		// shake screen
		shakeCamera.Shake(5f, 0.05f);

		// spawn bullet hit effect
		Instantiate(bulletHitFX, collider.gameObject.transform.position, collider.gameObject.transform.rotation);

		// Destroy the enemy bullet
		collider.gameObject.transform.parent.GetComponent<projectile_destroy>().Despawn();
	}
}
