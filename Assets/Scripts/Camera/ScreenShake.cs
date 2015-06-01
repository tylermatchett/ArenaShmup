using UnityEngine;
using System.Collections;

public class ScreenShake: MonoBehaviour
{
	public float magnitude = 5.0f;
	public float duration = 1.0f;
	//public GameObject map;
	private bool shaking;
	private float shakeMagnitude;
	private float shakeDuration;
	private float shakeTimer;
	private Vector3 shakeOffset;

	private Vector3 originalCameraPosition;

	public bool debugCauseShake = false;

	// Update is called once per frame
	void Update ()
	{
		if (debugCauseShake)
		{
			// Mouse left clicked, shake
			Shake(magnitude, duration);
			debugCauseShake = false;
		}

		if (shaking)
		{
			// Move our timer ahead based on the elapsed time
			shakeTimer += Time.deltaTime;
			
			// If we're at the max duration, we're not going to be shaking anymore
			if (shakeTimer >= shakeDuration)
			{
				shaking = false;
				shakeTimer = shakeDuration;
			}
			
			// Compute our progress in a [0, 1] range
			float progress = shakeTimer / shakeDuration;
			
			// Compute our magnitude based on our maximum value and our progress. This causes
			// the shake to reduce in magnitude as time moves on, giving us a smooth transition
			// back to being stationary. We use progress * progress to have a non-linear fall 
			// off of our magnitude. We could switch that with just progress if we want a linear 
			// fall off.
			float magnitude = shakeMagnitude * (1f - (progress * progress));
			
			// Generate a new offset vector with three random values and our magnitude
			shakeOffset = new Vector3(NextFloat(), NextFloat(), 0f) * magnitude;
			
			// If we're shaking, add our offset to our position and target
			shakeOffset = Camera.main.transform.TransformDirection(shakeOffset);
			//shakeOffset.y = 0;
			shakeOffset = originalCameraPosition + shakeOffset;

			Camera.main.transform.position = Vector3.MoveTowards(originalCameraPosition, shakeOffset, Time.deltaTime * magnitude);
			//camera.transform.position += shakeOffset;
		}
	}
	
	// Helper to generate a random float in the range of [-1, 1].
	private float NextFloat()
	{
		return (float)Random.value * 2f - 1f;
	}
	
	// Shakes the camera with a specific magnitude and duration.
	public void Shake(float magnitude, float duration)
	{
		// Camera starting position before the shake
		originalCameraPosition = Camera.main.transform.position;

		// We're now shaking
		shaking = true;
		
		shakeMagnitude = magnitude;
		shakeDuration = duration;
		shakeTimer = 0f;
	}
}