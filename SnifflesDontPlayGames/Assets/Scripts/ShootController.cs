using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {

	public GameObject bulletPrefab;

	private static int numBullets = 30;
	private GameObject[] bullets = new GameObject[numBullets];
	private int next;
	public float bulletVelocity = 10.0f;
	private GameObject currentBullet;
	private float bulletSpawnOffsetX = -16f; // Dragon mouth X offset from center
	private float bulletSpawnOffsetY = 2f; // Dragon mouth Y offset from center

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


		if (Input.GetMouseButtonDown(0)) {

			GameObject bullet = bullets[next++];
			if (next >= bullets.Length) {
				next = 0;
			}

			bullet.SetActive(true);
			bullet.transform.rotation = Quaternion.identity;
			currentBullet = bullet;

			// Shoot bullet in direction of cursor is
			Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			Vector2 myPos = new Vector2(transform.position.x,transform.position.y);
			Vector2 direction = cursorInWorldPos - myPos;
			direction.Normalize();
			currentBullet.transform.position = new Vector3(transform.position.x + bulletSpawnOffsetX, transform.position.y + bulletSpawnOffsetY, transform.position.z);
			currentBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletVelocity;
		}
			
	}

	public void DisableShooting() {
		canShoot = false;
	}

	public void EnableShooting() {
		canShoot = true;
	}
}
