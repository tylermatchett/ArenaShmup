using UnityEngine;
using System.Collections;

public class PlayerMovementManager : MonoBehaviour {

	PlayerManager playerManager;
	
	[Header ("Camera Bounds")]
	public float speedBufferX = 1f;
	public float speedBufferY = 0.25f;
	public float forceBufferX = 1f;
	public float forceBufferY = 0.25f;
	Vector2 boundsForce = Vector2.zero;
	float leftBound;
	float rightBound;
	float topBound;
	float bottomBound;

	Vector2 Direction;
	[Header ("Player Movement")]
	public float speed = 75f;
	public float BoostForce = 25f;
	public float drag = 0.9f;

	Rigidbody2D rigid;

	void Start() {
		rigid = GetComponent<Rigidbody2D>();
		playerManager = GetComponent<PlayerManager>();
	}

	void Update () {
		if (playerManager.playerState != PlayerManager.PlayerState.Paused) {
			CalculateBounds ();
			rigid.velocity += Direction * speed * GameManager.Instance.globalSpeedModifier * Time.deltaTime;
			CalculateBoundsForce ();
			rigid.velocity *= drag;
		}
	}

	void CalculateBoundsForce() {
		// Only while the match is inSession otherwise it drags in at the start
		boundsForce = Vector2.zero;
		if (transform.position.x < leftBound) {
			//    <---|- past the left area
			boundsForce.x -= speed * ((transform.position.x - leftBound) / forceBufferX) * Time.deltaTime;
		}
		if (transform.position.x > rightBound) {
			//    -|---> past the right area
			boundsForce.x -= speed * ((transform.position.x - rightBound) / forceBufferX) * Time.deltaTime;
		}
		if (transform.position.y < topBound) {
			// <---|- past the left area indicator
			boundsForce.y -= speed * ((transform.position.y - topBound) / forceBufferY) * Time.deltaTime;
		}
		if (transform.position.y > bottomBound) {
			// <---|- past the left area indicator
			boundsForce.y -= speed * ((transform.position.y - bottomBound) / forceBufferY) * Time.deltaTime;
		}
		
		rigid.velocity += boundsForce;
	}

	void MoveDirectionUpdate(Vector2 dir) {
		if (dir.magnitude > 1f) {
			Direction = dir.normalized;
		} else {
			Direction = dir;
		}
	}

	void Boost() {
		rigid.AddForce(Direction * BoostForce * GameManager.Instance.globalSpeedModifier, ForceMode2D.Impulse);
	}
	
	void CalculateBounds() {
		// Inner Bounds
		topBound = Camera.main.transform.position.y - Camera.main.orthographicSize + speedBufferY;
		bottomBound = Camera.main.transform.position.y + Camera.main.orthographicSize - speedBufferY;
		
		leftBound = Camera.main.transform.position.x - (Camera.main.orthographicSize * Screen.width / Screen.height) + speedBufferX;
		rightBound = Camera.main.transform.position.x + (Camera.main.orthographicSize * Screen.width / Screen.height) - speedBufferX;
		
		Debug.DrawLine (new Vector3 (leftBound, topBound, 0f), new Vector3 (rightBound, topBound, 0f), Color.red);
		Debug.DrawLine (new Vector3 (rightBound, topBound, 0f), new Vector3 (rightBound, bottomBound, 0f), Color.red);
		Debug.DrawLine (new Vector3 (rightBound, bottomBound, 0f), new Vector3 (leftBound, bottomBound, 0f), Color.red);
		Debug.DrawLine (new Vector3 (leftBound, bottomBound, 0f), new Vector3 (leftBound, topBound, 0f), Color.red);
	}

	public void ResetMovement() {
		rigid.velocity = Vector2.zero;
		Direction = (gameObject.transform.position - new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f)).normalized;
	}
}
