using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {

	public GameObject bulletPrefab;


	private static int numBullets = 30;
	private GameObject[] bullets = new GameObject[numBullets];
	private int next;
	private float bulletVelocity = 20.0f;
	private GameObject currentBullet;
	private float bulletSpawnOffset = 0.5f;
	private bool canShoot = true;

	public void DisableShooting()
	{
		canShoot = false;
	}

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
		if (Input.GetKeyDown(KeyCode.Space)) {
			
			GameObject bullet = bullets[next++];
			if (next >= bullets.Length) {
	
				// Delete old bullets and re-instantiate
				for (int i = 0; i < bullets.Length; i++)
					Destroy(bullets[i]);

				Start();
			}

			bullet.SetActive(true);
			bullet.transform.rotation = Quaternion.identity;
			currentBullet = bullet;

			// Shoot in direction player is trying to face
			if (Input.GetAxis("Vertical") > 0)
				ShootUp();
			else if (Input.GetAxis("Horizontal") < 0)
				ShootLeft();
			else if (Input.GetAxis("Horizontal") > 0)
				ShootRight();
			else
				ShootDown();
		}
			
	}

	void ShootUp() {
		currentBullet.transform.position = new Vector3(transform.position.x, transform.position.y + bulletSpawnOffset, transform.position.z);
		currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(0f, bulletVelocity, 0f);
	}

	void ShootDown() {
		currentBullet.transform.position = new Vector3(transform.position.x, transform.position.y - bulletSpawnOffset, transform.position.z);
		currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(0f, -bulletVelocity, 0f);
	}

	void ShootLeft() {
		currentBullet.transform.position = new Vector3(transform.position.x - bulletSpawnOffset, transform.position.y, transform.position.z);
		currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(-bulletVelocity, 0f, 0f);
	}

	void ShootRight() {
		currentBullet.transform.position = new Vector3(transform.position.x + bulletSpawnOffset, transform.position.y, transform.position.z);
		currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(bulletVelocity, 0f, 0f);
	}
}
