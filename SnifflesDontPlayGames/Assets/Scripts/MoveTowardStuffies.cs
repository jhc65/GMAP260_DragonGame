using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardStuffies : MonoBehaviour {

    public float speed = 5f;
    private int hp = 1;
    private FacingDir currentDir;
    private Transform target; 
	private GameObject player;

	private Animator anim; 
	int dirHash;

    void Start()
    {
		anim = GetComponent<Animator>();
		dirHash = Animator.StringToHash("Dir");

		target = GameObject.FindGameObjectWithTag("Stuffy").transform;
		player = GameObject.FindGameObjectWithTag("Player"); // needed for spawning explosition (bad for now)
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

		if (target == null) {
			print("Error. No target!");
            return leftDir;

		}

        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 toTarget = target.position - transform.position;
        float leftDotProduct = Vector3.Dot(left, toTarget);
        if (leftDotProduct < 0)  { 
			return rightDir; // target is to the right
        } else if (leftDotProduct > 0) { 
			return leftDir; // target is to the left
		} else { // On the target
            return leftDir; 
        }

    }

    void Update()
    {
        if (target == null)
            return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		SetAnimationDirection(GetDirectionToTarget());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {

            // Remove bullet
            collision.gameObject.SetActive(false);
            // Spawn explosition
            ShootController sc = player.GetComponent<ShootController>();
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
