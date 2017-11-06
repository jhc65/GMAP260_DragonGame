using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {

	public GameObject bulletPrefab;

	private static int numBullets = 30;
	private GameObject[] bullets = new GameObject[numBullets];
	private int next;
	public float bulletVelocity = 8000.0f;
	private GameObject currentBullet;
	private float bulletSpawnOffsetX = -16f; // Dragon mouth X offset from center (defaulted to facing left)
	private float bulletSpawnOffsetY = 2f; // Dragon mouth Y offset from center (defaulted to facing left)

	private bool canShoot = true;

	// Init array of bullets
	void Start () {
		for (int i = 0; i < bullets.Length; i++) {
			bullets[i] = (GameObject)Instantiate(bulletPrefab);
			bullets[i].SetActive(false);
		}
		next = 0;
	}

	// Update is called once per frame
	void Update () {
		if (!canShoot) return;

		// Fire on click
		if (Input.GetMouseButtonDown(0)) {

			GameObject bullet = bullets[next++];
			if (next >= bullets.Length) {
				next = 0;
			}

			bullet.SetActive(true);
			currentBullet = bullet;

			// Shoot bullet in direction of cursor is
			Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 myPos = new Vector2(transform.position.x,transform.position.y);
			Vector2 direction = cursorInWorldPos - myPos;

			direction.Normalize();

			// Choose where to spawn fire from (left side of mouth or right)
			if (GetComponent<PlayerController>().GetPlayerDirection().Equals(Dirs.right)) {
				bulletSpawnOffsetX = 1 * Mathf.Abs(bulletSpawnOffsetX);
				print("Facing right. Gonna use " + (transform.position.x + bulletSpawnOffsetX));
			}
			else if (GetComponent<PlayerController>().GetPlayerDirection().Equals(Dirs.left)) {
				bulletSpawnOffsetX = -1 * Mathf.Abs(bulletSpawnOffsetX);
				print("Facing left. Gonna use " + (transform.position.x + bulletSpawnOffsetX));
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

	public void DisableShooting() {
		canShoot = false;
	}

	public void EnableShooting() {
		canShoot = true;
	}
}
