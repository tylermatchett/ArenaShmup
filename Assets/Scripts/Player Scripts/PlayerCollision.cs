using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

	PlayerManager playerManager;
	public GameObject bulletHitFX;

	void Start() {
		playerManager = GetComponent<PlayerManager>();
	}

	void OnCollisionEnter2D(Collision2D collision) {

	}

	void OnTriggerEnter2D(Collider2D collider) {
		// Check team in here too
		if (collider.gameObject.transform.parent.GetComponent<projectile_stats> ().playerReference == gameObject) {
			//Debug.Log ("trigger: Hit Self");
			// Bullet Hit and it belongs to self
		} else if (collider.gameObject.transform.parent.GetComponent<projectile_stats> ().playerReference.GetComponent<PlayerManager> ().player.TeamID == gameObject.GetComponent<PlayerManager> ().player.TeamID) {
			// Friendly Bullet
			if (GameManager.Instance.FriendlyFire) {
				Debug.Log ("FF: Teammate dealt " + collider.gameObject.transform.parent.GetComponent<projectile_stats> ().damage + " to you.");
			}
		} else {
			playerManager.TakeDamage (collider.gameObject.transform.parent.GetComponent<projectile_stats> ().damage);
			// Flash the player white
			// shake screen
			// spawn bullet hit effect
			Instantiate (bulletHitFX, collider.gameObject.transform.position, collider.gameObject.transform.rotation);

			// store reference of the player that hit for kill/death tracking
			playerManager.lastPlayerToHitMe = collider.gameObject.transform.parent.GetComponent<projectile_stats> ().playerReference;

			// Destroy the enemy bullet
			collider.gameObject.transform.parent.GetComponent<projectile_destroy> ().Despawn ();
		}
	}
}
