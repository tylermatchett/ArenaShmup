using UnityEngine;
using System.Collections;

public class projectile_shakeScreenOnDetonate : MonoBehaviour {
	
	ScreenShake shakeCamera;
	
	public void Start() {
		shakeCamera = Camera.main.GetComponent<ScreenShake>();
	}

	public void DetonateShake() {
		shakeCamera.Shake(10f, 0.05f);
	}
}
