﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour {

	public float speed = 5f;
	public float fireBallSlowDown = 20f;
	public float fireBallShrinkFactor = .1f;

	private int hp = 1;
	private FacingDir currentDir;
	private Transform target; // This is set to private since making it public would require prefabbing the player...and then the text...this is just easier for now

	private Animator anim; 
	int dirHash;

	void Start () {
		anim = GetComponent<Animator>();
		dirHash = Animator.StringToHash("Dir");
		target = GameObject.FindGameObjectWithTag("Player").transform;
		currentDir = new FacingDir();
		currentDir = GetDirectionToTarget();
		SetAnimationDirection(currentDir);
	}


	void SetAnimationDirection(FacingDir d) {
		anim.SetInteger(dirHash, d.GetInt());
	}


	// Get the direction to the target.
	FacingDir GetDirectionToTarget() {
		FacingDir leftDir = new FacingDir("left");
		FacingDir rightDir = new FacingDir("right");
		FacingDir upDir = new FacingDir("up");
		FacingDir downDir = new FacingDir("down");

		if (target == null) {
			Debug.Log("Error. No target!");
			return leftDir;
		}

		// Check horizontal
		Vector3 left = transform.TransformDirection(Vector3.left);
		Vector3 toTarget = target.position - transform.position;
		float leftDotProduct = Vector3.Dot(left, toTarget);

		FacingDir favoredDirHoriz = new FacingDir("left");
		if (leftDotProduct < 0)  { 
			favoredDirHoriz = rightDir; // target is to the right
		} // (otherwise it's left)

		// Check vertical
		Vector3 down = transform.TransformDirection(Vector3.down);
		float downDotProduct = Vector3.Dot(down, toTarget);
		FacingDir favoredDirVert = new FacingDir("down");
		if (downDotProduct < 0)  { 
			favoredDirVert = upDir; // target is up
		} // (otherwise it's down)

		// Check if the direction is more horizontal or more vertical
		if (Mathf.Abs(leftDotProduct) >= Mathf.Abs(downDotProduct))
			return favoredDirHoriz;
		else
			return favoredDirVert;
	}


	void Update () {
		if (target == null)
			return;
		transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		SetAnimationDirection(GetDirectionToTarget());
	}
	public void Die() {
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Projectile")) {

			Vector2 projectileVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
			Vector2 oppositeForce = -projectileVelocity;

			if (projectileVelocity.magnitude < fireBallSlowDown) {
				collision.gameObject.SetActive(false);
			}
			else { 			

				// Slow the projectile
				collision.gameObject.GetComponent<Rigidbody2D>().AddForce(oppositeForce * fireBallSlowDown);
				//collision.gameObject.transform.localScale -= new Vector3(0, fireBallShrinkFactor, fireBallShrinkFactor);

			}

			// Spawn explosition at collision point
			ShootController sc = target.gameObject.GetComponentInChildren<ShootController>();
			sc.SpawnExplosion(gameObject.transform.position, 0);
			hp--;
			if (hp <= 0) {
				Die();
			}
		}
        else if (collision.gameObject.CompareTag("Explosion")) {
			print("ouch");
            hp--;
            if (hp <= 0) {
				Die();
            }
        }
	}

	public float GetEnemyMoveSpeed()
	{
		return speed;
	}

	public void ChangeEnemyMoveSpeed(float newSpeedIn)
	{
		speed = newSpeedIn;
	}

	public int GetEnemyHealth()
	{
		return hp;
	}

	public void ChangeEnemyHealth(int newHealthIn)
	{
		hp = newHealthIn;
	}
}
