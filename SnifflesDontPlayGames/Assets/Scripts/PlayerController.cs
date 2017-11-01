using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float playerSpeed = 4f;
	private int hp = 5;
	private bool canMove = true;

	void Start () {
		GameObject text = GameObject.Find("Game Over Text");
		GameObject text2 = GameObject.Find("Victory Text");
		text.GetComponent<Text>().enabled = false;
		text2.GetComponent<Text>().enabled = false;
	}

	void FixedUpdate () {
		if (!canMove)
			return;

        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        GetComponent<Rigidbody>().velocity = targetVelocity * playerSpeed;

		// Check for win
		if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
			GameObject text = GameObject.Find("Victory Text");
			text.GetComponent<Text>().enabled = true;
		}
    }

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Enemy") {
			hp--;
			if (hp <= 0) { 
				canMove = false;
				ShootController sc = GetComponent<ShootController>();
				sc.DisableShooting();
				GameObject text = GameObject.Find("Game Over Text");
				text.GetComponent<Text>().enabled = true;
			}
		}
	}
}
