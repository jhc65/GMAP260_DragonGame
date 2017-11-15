using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour {

	public float speed = 5f;
	private int hp = 1;
	private FacingDir currentDir;
	private Transform target; // This is set to private since making it public would require prefabbing the player...and then the text...this is just easier for now

	private Animator anim; 
	int dirHash;

	void Start () {
		anim = GetComponent<Animator>();
		dirHash = Animator.StringToHash("Dir");
		currentDir = new FacingDir();
		currentDir = GetDirectionToTarget();
		target = GameObject.FindGameObjectWithTag("Player").transform;
		SetAnimationDirection(currentDir);
	}


	void SetAnimationDirection(FacingDir d) {
		anim.SetInteger(dirHash, d.GetInt());
	}


	// Get the direction to the target.
	FacingDir GetDirectionToTarget() {
		FacingDir leftDir = new FacingDir("left");
		FacingDir rightDir = new FacingDir("right");

		if (target == null)
			return leftDir;
		
		Vector3 left = transform.TransformDirection(Vector3.left);
		Vector3 toTarget = target.position - transform.position;
		float leftDotProduct = Vector3.Dot(left, toTarget);
		if (leftDotProduct < 0) { // target is to the right
			return rightDir;
		}	
		else if (leftDotProduct > 0) { // target is to the right
			return leftDir;
		}
		else
			return leftDir; // target is on the player
	}


	void Update () {
		if (target == null)
			return;
		transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		SetAnimationDirection(GetDirectionToTarget());
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Projectile")) {

			// Remove bullet
			collision.gameObject.SetActive(false);
			// Spawn explosition
			ShootController sc = target.gameObject.GetComponent<ShootController>();
			sc.SpawnExplosion(gameObject.transform.position);
			hp--;
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
