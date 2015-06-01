using UnityEngine;
using System.Collections;

public class PlayerFacingDirection : MonoBehaviour {
	
	PlayerManager playerManager;

	Vector2 AimDirection;
	Vector2 MoveDirection;

	Vector2 FacingDirection;

	void Start () {
		playerManager = GetComponent<PlayerManager>();
	}

	void Update () {
		if (AimDirection.magnitude > 0.05f) {
			FacingDirection = AimDirection.normalized;
		} else if (MoveDirection.magnitude > 0.05f) {
			FacingDirection = MoveDirection.normalized;
		}

		float angle = Mathf.Atan2(FacingDirection.y, FacingDirection.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
	
	void MoveDirectionUpdate(Vector2 dir) {
		MoveDirection = dir;
	}
	
	void AimDirectionUpdate(Vector2 dir) {
		AimDirection = dir;
	}
}
