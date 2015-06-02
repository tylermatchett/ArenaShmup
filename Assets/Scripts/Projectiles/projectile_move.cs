using UnityEngine;
using System.Collections;

public class projectile_move : MonoBehaviour {

	public Vector2 direction;
	public float speed = 100;
	public float drag = 0.9f;

	Vector3 velocity;

	void Update () {
		velocity = direction.normalized * speed * GameManager.Instance.globalBulletSpeedModifier * Time.deltaTime;

		GetComponent<Rigidbody2D>().velocity = velocity;
		GetComponent<Rigidbody2D>().velocity *= drag;

		FaceDirection();
	}

	public void FaceDirection() {
		float angle = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (GetComponent<projectile_stats> ().playerReference != collision.gameObject) {
			Vector2 reflect = direction - 2f * Vector2.Dot (direction, collision.contacts [0].normal) * collision.contacts [0].normal;
			direction = reflect;
		}
	}
}
