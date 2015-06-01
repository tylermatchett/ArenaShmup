using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public float totalHealth;
	public float currentHealth;

	Image healthScale;

	public void Update() {
	}

	public void ResetStats() {
		currentHealth = totalHealth;
		healthScale.rectTransform.localScale = new Vector3((currentHealth / totalHealth), healthScale.rectTransform.localScale.y, healthScale.rectTransform.localScale.z);
	}

	public void TakeDamage(float dmg) {
		currentHealth -= dmg;
		healthScale.rectTransform.localScale = new Vector3((currentHealth / totalHealth), healthScale.rectTransform.localScale.y, healthScale.rectTransform.localScale.z);
	}
	
	public bool CheckDeath() {
		if (currentHealth <= 0) {
			return true;
		}

		return false;
	}

	void UIHealthControlsImage(Image img) {
		healthScale = img;
	}
}
