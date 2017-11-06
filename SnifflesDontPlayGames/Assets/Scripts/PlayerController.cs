﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float playerSpeed = 4f;
    private int maxHealth = 5;
	private int hp = 5;
	private bool canMove = true;
	private FacingDir currentDir;


    public GameObject gameOverText;
	public GameObject victoryText;
    public Text playerhealthUI;

	void Start () {
        playerhealthUI.text = hp.ToString();
		currentDir = new FacingDir("left");
    }

	void FlipHorizontal() {
		if (currentDir == null)
			return;

		// Switch the way the player is labelled as facing
		currentDir.Flip();

		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void FixedUpdate () {
		if (!canMove)
			return;

		float horiz = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");
        Vector3 targetVelocity = new Vector3(horiz, vert);

		if (currentDir.IsLeft() && horiz > 0) {
			FlipHorizontal();
		}
		if (currentDir.IsRight() && horiz < 0) {
			FlipHorizontal();
		}

        GetComponent<Rigidbody2D>().velocity = targetVelocity * playerSpeed;


		// Check for win
		if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
			victoryText.GetComponent<Text>().enabled = true;
		}
    }

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Enemy") {
			hp--;
            playerhealthUI.text = hp.ToString();
			if (hp <= 0) { 
				canMove = false;
				ShootController sc = GetComponent<ShootController>();
				sc.DisableShooting();
				gameOverText.GetComponent<Text>().enabled = true;
				EnemySpawner es = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();
				es.Disable();
				gameOverText.GetComponent<Text>().enabled = true;
			}
		}
	}

    public float GetPlayerHealth()
    {
        return (float)hp;
    }

    public void ChangePlayerHealth(int newHealthIn)
    {
        playerhealthUI.text = newHealthIn.ToString();
        maxHealth = newHealthIn;
        hp = newHealthIn;
    }

    public float GetPlayerMovementSpeed()
    {
        return playerSpeed;
    }

    public void ChangePlayerMovementSpeed(float newMoveSpeedIn)
    {
        playerSpeed = newMoveSpeedIn;
    }

	public FacingDir GetPlayerDirection() {
		return currentDir;
	}

	public void SetPlayerDirection(FacingDir dir) {
		currentDir = dir;
	}

}
