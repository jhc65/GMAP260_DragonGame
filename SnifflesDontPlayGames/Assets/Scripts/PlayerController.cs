using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float playerSpeed = 4f;
	public float hp = 19;
	public float maxHP = 19;
	public int knightDamageReceieved = 2;
	private bool canMove = true;
	private bool isDead = false;

	// GUI
    public GameObject gameOverText;
	public GameObject victoryText;
    public Text playerhealthTextUI; // for debugging
	public Sprite[] healthBarImages;
	public SpriteRenderer healthBar;

	// Audio
	private AudioSource source;
	public AudioClip injurySound;

	// Animation
	private FacingDir currentDir;
	private Animator anim; 
	private int dirHash;
	private int deadHash;

	private FacingDir dirLeft;
	private FacingDir dirRight;
	private FacingDir dirUp;
	private FacingDir dirDown;

	void Start () {
		anim = GetComponent<Animator>();

		playerhealthTextUI.text = hp.ToString();
		dirLeft = new FacingDir("left");
		dirRight = new FacingDir("right");
		dirUp = new FacingDir("up");
		dirDown = new FacingDir("down");

		currentDir = dirLeft;
		dirHash = Animator.StringToHash("Dir");
		deadHash = Animator.StringToHash("isDead");
		SetAnimationDirection();
		source = GetComponent<AudioSource>();
    }

	void SetAnimationDirection() {
		anim.SetInteger(dirHash, currentDir.GetInt());
	}

	void TriggerDeathAnimation() {
		anim.SetBool(deadHash, true);
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
		} else if (vert < 0) {
			anim.enabled = true;
			currentDir = dirDown;
			SetAnimationDirection();
		} else if (vert > 0 ) {
			anim.enabled = true;
			currentDir = dirUp;
			SetAnimationDirection();
		} else { 
			// Not moving. Disable animation
			anim.enabled = false;

		}
	}
	void FixedUpdate () {
		if (!canMove) return;

		HandleMovement();
    }

	void StopActivity() {
		canMove = false;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		ShootController sc = GetComponentInChildren<ShootController>();
		sc.DisableShooting();
		EnemySpawner es = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();
		es.Disable();

		// Remove all enemies
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
			enemy.SetActive(false);
		}
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Stealer")) {
			enemy.SetActive(false);
		}

		TriggerDeathAnimation(); // show losing animation

	}

	// Called when something touches the player
	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Enemy")) {

			//play injury sound
			float vol = 1.0f;
			//source.PlayOneShot(injurySound, vol);
			source.Play();

			ApplyDamage(knightDamageReceieved);

		}
	}

	public void ApplyDamage(int dmg) {
		hp -= dmg;
		if (hp <= 0) { 
			healthBar.sprite = healthBarImages[healthBarImages.Length - 1];
			isDead = true;
			GameOver();
			return;
		}

		// Update health bar sprite to appropriate damage level
		if (hp > 1)
			healthBar.sprite = healthBarImages[(int) maxHP - (int) hp];
		playerhealthTextUI.text = hp.ToString();

	}
	public void GameOver() {
		gameOverText.GetComponent<Text>().enabled = true;
		if (isDead) {
			gameOverText.GetComponent<Text>().text = "You Died";
		}
		else {
			gameOverText.GetComponent<Text>().text = "Stuffies Lost";
		}
		StopActivity();
	}

    public float GetPlayerHealth() {
        return (float)hp;
    }

    public void ChangePlayerHealth(float newHealthIn) {
		playerhealthTextUI.text = newHealthIn.ToString();
		maxHP = newHealthIn;
        hp = newHealthIn;
    }

    public float GetPlayerMovementSpeed() {
        return playerSpeed;
    }

    public void ChangePlayerMovementSpeed(float newMoveSpeedIn) {
        playerSpeed = newMoveSpeedIn;
    }

	public FacingDir GetPlayerDirection() {
		return currentDir;
	}

	public void SetPlayerDirection(FacingDir dir) {
		currentDir = dir;
	}

}
