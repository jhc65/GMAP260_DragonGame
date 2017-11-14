﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float playerSpeed = 4f;
    private int maxHealth = 9999;
	private int hp = 9999;
	private bool canMove = true;
	private FacingDir currentDir;

    public GameObject gameOverText;
	public GameObject victoryText;
    public Text playerhealthUI;

	private AudioSource source;
	public AudioClip injurySound;

	public bool muted = true;

	void Start () {
        playerhealthUI.text = hp.ToString();
		currentDir = new FacingDir("left");

		source = GetComponent<AudioSource>();
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
		if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 &&
			GameObject.FindGameObjectsWithTag("Stealer").Length == 0) {
			// Commenting this out for dev purposes
			// victoryText.GetComponent<Text>().enabled = true;
			// StopActivity();
		}
    }

	void StopActivity() {
		canMove = false;
		ShootController sc = GetComponent<ShootController>();
		sc.DisableShooting();
		EnemySpawner es = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();
		es.Disable();
	}




	// Called when something touches the player
	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Enemy")) {

			//play injury sound
			float vol = 1.0f;
			//source.PlayOneShot(injurySound, vol);
			if (!muted) 
				source.Play();

			hp--;
            playerhealthUI.text = hp.ToString();
			if (hp <= 0) { 
				GameOver();
			}
		}

		// If a snuffie is touched, put it back
		if (collision.CompareTag("Dropped")) {
			collision.gameObject.GetComponent<Stuffy>().ReturnStuffy();
		}
	}

	public void GameOver() {
		gameOverText.GetComponent<Text>().enabled = true;
		StopActivity();
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
