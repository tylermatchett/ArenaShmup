using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Weapon : MonoBehaviour {
	[Header ("Name and Projectile")]
	public GameObject bulletPrefab;
    public string Name;
    public GameObject playerObj;
    public AudioClip shotSFX;
    public AudioClip emptyClipSFX;

    AudioSource audio;
	
	[Header ("Cooldowns")]
	public float totalCooldown;
	float cooldown;
	public float fireRateTotal;
	float fireRate;
	public int shotsPerBurst;
	int burstShots;
	public int totalAmmo;
	int ammo;
	public float reloadTimeTotal;
	float reloadTime;

	[Header ("Accuracy Variables")]
	public float shotSpread;
	public float accuracy;
	public float accuracyReductionDelay;
	[Range (0f, 1f)]
	public float accuracyWeight;
	float tempXSpread;
	float tempYSpread;

	[Header ("Projectile Stats")]
	public float damage;
	public float bulletLife;
	public float bulletSpeed;
	public int pooledBulletCount;
	
	List<GameObject> bulletPool;
	List<GameObject> bulletSpawns;
	int spawnLocationCounter = 0;
	int spawnLocationTotal = 0;
	
	[Header ("Debug Fire Weapon")]
	public Vector2 direction;
	public bool isFiring;
	bool reloading;
	bool canShoot;
	bool Fire;

	Vector2 AimDirection;
	Vector2 MoveDirection;

	Image ammoReloadScale;
	Text ammoRemainingText;
	float ammoScale = 1f;
	
	//ScreenShake shakeCamera;
	
	void Start() {
		/* Static Starting Variables */
		isFiring = false;
		canShoot = false;
		Fire = false;

		cooldown = totalCooldown;
		fireRate = fireRateTotal;
		burstShots = shotsPerBurst;
		ammo = totalAmmo;

		initializeBulletPool();
		GetBulletSpawnLocations();

        audio = transform.parent.GetComponent<AudioSource>();
        audio.volume = GameManager.Instance.Volume_SoundEffects * GameManager.Instance.Volume_Master;
        audio.clip = shotSFX;
		//shakeCamera = Camera.main.GetComponent<ScreenShake>();
	}

	public void ResetWeapon() {
		cooldown = totalCooldown;
		fireRate = fireRateTotal;
		burstShots = shotsPerBurst;
		ammo = totalAmmo;
		EndFire();
	}

	void initializeBulletPool() {
		bulletPool = new List<GameObject>();
		//Debug.Log (playerObj.name);
		for (int i = 0; i < pooledBulletCount; i++) {
			bulletPool.Add((GameObject) Instantiate(bulletPrefab));
			// TODO Loop through the player list and remove the cllision with the list (team mates in the case of a no FF game)
			//Physics2D.IgnoreCollision(bulletPool[i].GetComponent<Collider2D>(), playerObj.GetComponent<Collider2D>());
			bulletPool[i].GetComponent<projectile_stats>().playerReference = playerObj;
			bulletPool[i].GetComponent<projectile_destroy>().lifetime = bulletLife;
			bulletPool[i].GetComponent<projectile_move>().speed = bulletSpeed;
			bulletPool[i].SetActive(false);
			Physics2D.IgnoreCollision(bulletPool[i].GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}
	}

	void GetBulletSpawnLocations() {
		bulletSpawns = new List<GameObject>();
		foreach (Transform child in transform) {
			if (child.gameObject.tag == "bulletSpawnLocation") {
				bulletSpawns.Add(child.gameObject);
			}
		}
		spawnLocationTotal = bulletSpawns.Count;
		//Debug.Log("Getting " + spawnLocationTotal + " Spawn Locations");
	}
	
	void Update() {
		if (AimDirection.magnitude > 0.25f) {
			direction = AimDirection.normalized;
		} else if (MoveDirection.magnitude > 0.25f) {
			direction = MoveDirection.normalized;
		}

		cooldown += Time.deltaTime;
		if ((cooldown > totalCooldown) && (ammo > 0) && (!reloading)) {
			// Can shoot gun
			canShoot = true;
		} else if (ammo <= 0) {
			Reload();
		}
		
		if (reloading) {
            if (isFiring) {
                // Play Empty Clip Sound
                audio.clip = emptyClipSFX;
                audio.Play();
            }
			reloadTime += Time.deltaTime;
			if (reloadTime > reloadTimeTotal) {
				reloadTime = 0f;
				reloading = false;
				ammo = totalAmmo;
			}
			ammoScale = reloadTime / reloadTimeTotal;
			ammoReloadScale.rectTransform.localScale = new Vector3 (ammoScale, ammoReloadScale.rectTransform.localScale.y, ammoReloadScale.rectTransform.localScale.z);
			ammoRemainingText.text = "none";
		} else {
			if ((ammoReloadScale != null) && (ammoRemainingText != null)) {
				ammoScale = (float) ammo / totalAmmo;
				ammoReloadScale.rectTransform.localScale = new Vector3 (ammoScale, ammoReloadScale.rectTransform.localScale.y, ammoReloadScale.rectTransform.localScale.z);
				ammoRemainingText.text = ammo.ToString ();
			}
		}
		
		if (canShoot && isFiring) {
			// Can shoot
			Fire = true;
			cooldown = 0f;
		}
		
		if (Fire) {
			// Empty the clip
			fireRate += Time.deltaTime;
			if (fireRate > fireRateTotal) {
                audio.clip = shotSFX;
                if (fireRateTotal < 0.01f) {
                    if (burstShots == shotsPerBurst) {
                        // only play the audio once per round
                        audio.Play();
                    }
                } else {
                    audio.Play();
                }

                // Fire a shot
				Shoot();
				
				burstShots--;
				fireRate = 0f;
			}
			
			// if no ammo remains
			if (burstShots <= 0) {
				// Reset Weapon
				// Set the canShoot and Fire to false
				canShoot = false;
				Fire = false;
				// Set the ammo to totalAmmo
				burstShots = shotsPerBurst;
				ammo--;
			}
		}
		
		if (isFiring) {
			accuracy += Time.deltaTime;
			if (accuracy > accuracyReductionDelay) {
				accuracy = accuracyReductionDelay;
			}
		} else {
			accuracy -= Time.deltaTime;
			if (accuracy < 0f) {
				accuracy = 0f;
			}
		}
	}
	
	public void Reload() {
		reloading = true;
	}
	
	protected void Shoot() {
		// Grab a bullet off of the bullet stack
		GameObject tempBullet = GetFirstDisabledBullet();
		
		if (tempBullet != null) {
			tempBullet.transform.position = bulletSpawns[spawnLocationCounter % spawnLocationTotal].transform.position;
			spawnLocationCounter++;
			tempXSpread = Random.Range(-shotSpread, shotSpread);
			tempYSpread = Random.Range(-shotSpread, shotSpread);
			tempBullet.GetComponent<projectile_move>().direction = new Vector2(direction.x + ((tempXSpread * (1f - accuracyWeight)) + (tempXSpread * (accuracy / accuracyReductionDelay) * accuracyWeight)), direction.y + ((tempYSpread * (1f - accuracyWeight)) + (tempYSpread * (accuracy / accuracyReductionDelay) * accuracyWeight))).normalized;
			tempBullet.GetComponent<projectile_move>().FaceDirection();
			tempBullet.GetComponent<projectile_stats>().damage = damage;
			tempBullet.SetActive(true);
		}
	}

	public void SetPlayerObj(GameObject obj) {
		playerObj = obj;
	}
	
	GameObject GetFirstDisabledBullet() {
		for (int i = 0; i < bulletPool.Count; i++) {
			if (!bulletPool[i].activeSelf) {
				return bulletPool[i];
			}
		}
		
		Debug.LogError("Need more bullets in the " + Name + "'s prefab pool.");
		return null;
	}
	
	public void StartFire() {
		isFiring = true;
	}
	
	public void EndFire() {
		isFiring = false;
	}
	
	void MoveDirectionUpdate(Vector2 dir) {
		MoveDirection = dir;
	}
	
	void AimDirectionUpdate(Vector2 dir) {
		AimDirection = dir;
	}
	
	void UIWeaponControlsText(Text ammoNotificationText) {
		ammoRemainingText = ammoNotificationText;
	}
	
	void UIWeaponControlsImage(Image ammoScale) {
		ammoReloadScale = ammoScale;
	}
}