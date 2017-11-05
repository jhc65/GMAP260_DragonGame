using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float playerSpeed = 4f;
    private int maxHealth = 5;
	private int hp = 5;
	private bool canMove = true;

    public GameObject gameOverText;
    public GameObject victoryText;

    public Text playerhealthUI;

	void Start () {
        playerhealthUI.text = hp.ToString();
    }

	void FixedUpdate () {
		if (!canMove)
			return;

        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        GetComponent<Rigidbody>().velocity = targetVelocity * playerSpeed;

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
}
