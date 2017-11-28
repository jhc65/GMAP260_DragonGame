using System.Collections;
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
	private Animator anim; 
	private int dirHash;

	private bool muted = false;
	private FacingDir dirLeft;
	private FacingDir dirRight;

	void Start () {
		anim = GetComponent<Animator>();

        playerhealthUI.text = hp.ToString();
		dirLeft = new FacingDir("left");
		dirRight = new FacingDir("right");
		currentDir = dirLeft;
		dirHash = Animator.StringToHash("Dir");

		SetAnimationDirection();
		source = GetComponent<AudioSource>();
    }

	void SetAnimationDirection() {
		anim.SetInteger(dirHash, currentDir.GetInt());
	}

	void HandleMovement() {
		float horiz = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");
		Vector3 targetVelocity = new Vector3(horiz, vert);


		GetComponent<Rigidbody2D>().velocity = targetVelocity * playerSpeed;
		if (horiz < 0) {
			anim.enabled = true;
			currentDir = dirLeft;
			SetAnimationDirection();
		} else if (horiz > 0) {
			anim.enabled = true;

			currentDir = dirRight;
			SetAnimationDirection();
		} else { 
			// Not moving. Eventually display idle animation. For now, disable animation
			anim.enabled = false;

		}
	}
	void FixedUpdate () {
		if (!canMove) return;

		HandleMovement();

		// Check for win. Should be when waves end or never
		if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 &&
			GameObject.FindGameObjectsWithTag("Stealer").Length == 0) {
			// Commenting this out for dev purposes
			// victoryText.GetComponent<Text>().enabled = true;
			// StopActivity();
		}
    }

	void StopActivity() {
		canMove = false;
		muted = true;
		ShootController sc = GetComponentInChildren<ShootController>();
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
