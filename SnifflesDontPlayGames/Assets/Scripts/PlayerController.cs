using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float playerSpeed = 4f;
	public float hp = 10;
	public float maxHP = 10;
	private bool canMove = true;
	private FacingDir currentDir;

    public GameObject gameOverText;
	public GameObject victoryText;
    public Text playerhealthTextUI; // for debugging
	public Image playerHealthBarUI;

	private AudioSource source;
	public AudioClip injurySound;
	private Animator anim; 
	private int dirHash;

	private bool muted = false;
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

		// Check for win?

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
			playerhealthTextUI.text = hp.ToString();
			playerHealthBarUI.fillAmount = hp / maxHP;
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

    public void ChangePlayerHealth(float newHealthIn)
    {
		playerhealthTextUI.text = newHealthIn.ToString();
		maxHP = newHealthIn;
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
