using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	// Explosion to spawn after landing jump
	public GameObject AoE;

	// Movement
    public float playerMoveSpeed = 4f;
	public float playerJumpSpeed = 8f;

	// Health
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
	private int jumpHash;

	private FacingDir dirLeft;
	private FacingDir dirRight;
	private FacingDir dirUp;
	private FacingDir dirDown;

	private GameObject AoEToRemove;
	private bool isTouchingWall;
	private bool invincible;

	void Start () {
		anim = GetComponent<Animator>();
		invincible = false;
		playerhealthTextUI.text = hp.ToString();
		dirLeft = new FacingDir("left");
		dirRight = new FacingDir("right");
		dirUp = new FacingDir("up");
		dirDown = new FacingDir("down");

		currentDir = dirLeft;
		dirHash = Animator.StringToHash("Dir");
		deadHash = Animator.StringToHash("isDead");
		jumpHash = Animator.StringToHash("ChargeDir");
		SetAnimationDirection();
		source = GetComponent<AudioSource>();
    }

	void SetAnimationDirection() {
		anim.SetInteger(dirHash, currentDir.GetInt());
	}

	void TriggerDeathAnimation() {
		anim.SetBool(deadHash, true);
	}

	// Start jumping (see Dragon animation controller scripts)
	void TriggerJump() {
		anim.SetInteger(jumpHash, currentDir.GetInt());
	}

	void CheckForJump() {

		// Jump
		if (Input.GetKeyDown("space") && currentDir == dirLeft) {
			anim.enabled = true;
			DisableMovement();
			TriggerJump();
		}
	}

	void HandleMovement() {
		if (!canMove)
			return;
		
		float horiz = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");
		Vector3 targetVelocity = new Vector3(horiz, vert);

		GetComponent<Rigidbody2D>().velocity = targetVelocity * playerMoveSpeed;

		if (horiz == 0 && vert == 0) {
			anim.enabled = false;
			CheckForJump();
			return;
		}
		CheckForJump();

		// Face appropriate direction
		anim.enabled = true;

		Vector2 vel = GetComponent<Rigidbody2D>().velocity;
		if (vel.x < 0) {
			currentDir = dirLeft;
		} else if (vel.x > 0) {
			currentDir = dirRight;
		} else if (vel.y < 0) {
			currentDir = dirDown;
		} else if (vel.y > 0 ) {
			currentDir = dirUp;
		}
		SetAnimationDirection();

	}
	void FixedUpdate () {
		HandleMovement();
    }

	void StopActivity() {
		DisableMovement();
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
			if (!invincible)
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
		StopActivity();
		gameOverText.GetComponent<Text>().enabled = true;
		if (isDead) {
			gameOverText.GetComponent<Text>().text = "You Died";
		}
		else {
			gameOverText.GetComponent<Text>().text = "Stuffies Lost";
		}
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
		return playerMoveSpeed;
    }

    public void ChangePlayerMovementSpeed(float newMoveSpeedIn) {
		playerMoveSpeed = newMoveSpeedIn;
    }

	public FacingDir GetPlayerDirection() {
		return currentDir;
	}

	public void SetPlayerDirection(FacingDir dir) {
		currentDir = dir;
	}

	public void DisableMovement() {
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		canMove = false;
	}
		
	public void EnableMovement() {
		canMove = true;
	}

	public void SpawnExplosion() {
		GameObject aoe = GameObject.Instantiate(AoE);
		aoe.transform.position = gameObject.transform.position;
		aoe.GetComponent<AoE>().TriggerExplosionDamage(40);
		AoEToRemove = aoe; // save to later remove the aoe effect
	}

	public void RemoveExplosion() {
		Destroy(AoEToRemove);
	}

	public void SetInvincible(bool isInvincible) {
		invincible = isInvincible;
	}
}
