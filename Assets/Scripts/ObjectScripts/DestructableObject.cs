using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour {

	public float health;
	public GameObject objectExplosion;

	public void TakeDamage(float dmg) {
		health -= dmg;
		if (CheckDeath ()) {
			// Destroy with debris
			Instantiate(objectExplosion, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject, 0.1f);
		}
	}
	
	public bool CheckDeath() {
		if (health <= 0) {
			return true;
		}
		
		return false;
	}
}
