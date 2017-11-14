using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {

	public GameObject bulletPrefab;
	public GameObject explosionPrefab;

	public AudioClip shootSound;
	public bool muted = true;

	private static int numBullets = 30;
	private GameObject[] bullets = new GameObject[numBullets];
	//private GameObject[] explosions = new GameObject[numBullets];

	private int nextBullet;
	private int nextExplosion;
	public float bulletVelocity = 8000.0f;
	private GameObject currentBullet;
	private float bulletSpawnOffsetX = -16f; // Dragon mouth X offset from center (defaulted to facing left)
	private float bulletSpawnOffsetY = 2f; // Dragon mouth Y offset from center (defaulted to facing left)

	private AudioSource source;

	private bool canShoot = true;

	// Init array of bullets
	void Start () {
		for (int i = 0; i < bullets.Length; i++) {
			bullets[i] = (GameObject)Instantiate(bulletPrefab);
			bullets[i].SetActive(false);
		//	explosions[i] = (GameObject)Instantiate(explosionPrefab);
		//	explosions[i].SetActive(false);
		}
		nextBullet = 0;

		source = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		if (!canShoot) return;
		// Fire on click
		if (Input.GetMouseButtonDown(0)) {

			GameObject bullet = bullets[nextBullet++];
			if (nextBullet >= bullets.Length) {
				nextBullet = 0;
			}

			bullet.SetActive(true);
			currentBullet = bullet;

			//trigger audio
			//float vol = 1.0f;
			//source.PlayOneShot(shootSound,vol);


			// Shoot bullet in direction of cursor is
			Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 myPos = new Vector2(transform.position.x,transform.position.y);
			Vector2 direction = cursorInWorldPos - myPos;

			direction.Normalize();

			// Choose where to spawn fire from (left side of mouth or right)
			if (GetComponent<PlayerController>().GetPlayerDirection().Equals(Dirs.right)) {
				bulletSpawnOffsetX = 1 * Mathf.Abs(bulletSpawnOffsetX);
			}
			else if (GetComponent<PlayerController>().GetPlayerDirection().Equals(Dirs.left)) {
				bulletSpawnOffsetX = -1 * Mathf.Abs(bulletSpawnOffsetX);
			}

			currentBullet.transform.position = new Vector3(transform.position.x + bulletSpawnOffsetX, transform.position.y + bulletSpawnOffsetY, transform.position.z);
			currentBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletVelocity;
			Vector3 vel = currentBullet.GetComponent<Rigidbody2D>().velocity;

			// Face fireball to the direction it is traveling
			// Left
			if (direction.x < 0) {
				currentBullet.transform.Rotate(new Vector3(0f,0f, 180 - vel.y));			
			}

			// Right
			else { 
				currentBullet.transform.Rotate(new Vector3(0f,0f, vel.y));
			}
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
