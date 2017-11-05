using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour {

	public Transform target;
	public float speed = 5f;
	private int hp = 3;
	private FacingDir currentDir;

	void Start () {
		currentDir = new FacingDir();
		currentDir = GetDirectionToTarget();
	}

	// Get the direction to the target. Optional argument if you want the return direction flipped
	FacingDir GetDirectionToTarget(bool flip = false) {
		Vector3 left = transform.TransformDirection(Vector3.left);
		Vector3 toTarget = target.position - transform.position;
		float leftDotProduct = Vector3.Dot(left, toTarget);
		if (leftDotProduct < 0) { // target is to the right
			if (flip)
				return Dirs.left; 
			else
				return Dirs.right;
		}	
		else if (leftDotProduct > 0) { // target is to the right
			if (flip)
				return Dirs.right; 
			else
				return Dirs.left;
		}
		else
			return Dirs.left; // target is on the player
	}

	// Flip transform
	void FlipDir(FacingDir toDir)
	{
		if (currentDir == null || toDir.Equals(currentDir))
			return;

		// Switch the way the player is labelled as facing
		currentDir.Flip();

		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		FlipDir(GetDirectionToTarget(true));
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Projectile") {

			// Remove bullet
			collision.collider.gameObject.SetActive(false);

			hp--;
			// GetComponent<Renderer>().material.color = hitColor; // Effect to show enemy was hit. Change color to white
			if (hp <= 0)
				Destroy(gameObject);

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
