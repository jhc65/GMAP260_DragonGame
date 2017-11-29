using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {

	public GameObject bulletPrefab;
	public GameObject explosionPrefab;
	public float[] velocities;
	public int chargeSpeed = 100;
	public int maxCharge = 1000;
	public float minimumFiringVelocity = 20f;
	private static int numBullets = 30;
	private GameObject[] bullets = new GameObject[numBullets];
	//private GameObject[] explosions = new GameObject[numBullets];

	private int nextBullet;
	private int nextExplosion;

	private GameObject currentBullet;
	private float bulletSpawnOffsetX = -16f; // Dragon mouth X offset from center (defaulted to facing left)
	private float bulletSpawnOffsetY = 2f; // Dragon mouth Y offset from center (defaulted to facing left)

	public AudioClip fireballSound;
	private AudioSource audio;

	private bool canShoot = true;
	private float chargeLevel = 0f;

	void Start () {
		// Init array of bullets
		for (int i = 0; i < bullets.Length; i++) {
			bullets[i] = (GameObject)Instantiate(bulletPrefab);
			bullets[i].SetActive(false);
		//	explosions[i] = (GameObject)Instantiate(explosionPrefab);
		//	explosions[i].SetActive(false);
		}
		nextBullet = 0;

		audio = GetComponent<AudioSource>();
	}

	void UpdateMouthPosition() {
		FacingDir dragonDirection = transform.parent.GetComponent<PlayerController>().GetPlayerDirection();
		if (dragonDirection.Equals(Dirs.right))
			transform.localPosition = new Vector3(13, 10, -1);
		else if (dragonDirection.Equals(Dirs.left))
			transform.localPosition = new Vector3(-14, 10, -1);
	}

	// convert charge level to velocity
	float GetCurrentVelocityFromChargeLevel() {
		return chargeLevel;
	}


	// Update is called once per frame
	void Update () {
		if (!canShoot) return;

		UpdateMouthPosition();

		// Start charging on mouse hold
		if (Input.GetMouseButton(0)) {

			GameObject bullet = bullets[nextBullet++];
			currentBullet = bullet;
			if (nextBullet >= bullets.Length) {
				nextBullet = 0;
			}
			chargeLevel += Time.deltaTime * chargeSpeed;
			chargeLevel = (chargeLevel > maxCharge - 1 ? maxCharge : chargeLevel);
			//print(chargeLevel);

		}
		// Release mouse and fire
		if (Input.GetMouseButtonUp(0))
		{
			float bulletVelocity = GetCurrentVelocityFromChargeLevel();
			if (bulletVelocity < minimumFiringVelocity) return;

			//Debug.Log(bulletVelocity);
			chargeLevel = 0;
			currentBullet.SetActive(true);

			//PlaySound (fireballSound, transform.position);
			CustomAudio.PlaySound(fireballSound, transform.position);


			// Shoot bullet in direction of cursor is
			Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 myPos = new Vector2(transform.position.x,transform.position.y);
			Vector2 direction = cursorInWorldPos - myPos;
			Vector2 distToTravel;
			float timeToTravel;
			direction.Normalize();
			currentBullet.transform.position = transform.position;
			distToTravel = new Vector2((cursorInWorldPos.x - currentBullet.transform.position.x), (cursorInWorldPos.y - currentBullet.transform.position.y));
			currentBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletVelocity;
			Vector3 vel = currentBullet.GetComponent<Rigidbody2D>().velocity;
			timeToTravel = (Mathf.Sqrt(Mathf.Pow(distToTravel.x, 2) + Mathf.Pow(distToTravel.y, 2)) / Mathf.Sqrt(Mathf.Pow(vel.x, 2) + Mathf.Pow(vel.y, 2)));
			IEnumerator coroutine = BulletMovement(timeToTravel, currentBullet);
			StartCoroutine(coroutine);
		}


	}

    IEnumerator BulletMovement(float timeToTravel, GameObject movingBullet)
    {
        yield return new WaitForSeconds(timeToTravel);

        if (movingBullet.activeSelf) {
            movingBullet.SetActive(false);
            SpawnExplosion(movingBullet.transform.position);
        }
    }

    public void SpawnExplosion(Vector3 atPos) {
		/*
		GameObject explosion = explosions[nextExplosion++];
		if (nextExplosion >= explosions.Length) {
			nextExplosion = 0;
		}

		explosion.SetActive(true);
		*/
		GameObject explosion = GameObject.Instantiate(explosionPrefab);
		explosion.SetActive(true);
		explosion.transform.position = atPos;

		// Destroy this after animation is complete and small delay
		Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f); 
	}

	public void DisableShooting() {
		canShoot = false;
	}

	public void EnableShooting() {
		canShoot = true;
	}

}
