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
	private int dirHash;

	bool isHolding = false;
	private GameObject stuffyObj;

    void Start()
    {
		anim = GetComponent<Animator>();
		dirHash = Animator.StringToHash("Dir");
		player = GameObject.FindGameObjectWithTag("Player"); // needed for spawning explosition (bad for now)
		UpdateTarget();
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

	void UpdateTarget() {
		if (isHolding) { // If holding stuffy head towards the door
			target = GameObject.FindGameObjectWithTag("Door").transform;
			return;
		} else { 
			// if not holding stuffy, head towards ground stuffies as a first priority
			// if there are no stuffies on the ground, head towards the pile stuffies
			GameObject[] possibleStuffies = GameObject.FindGameObjectsWithTag("Stuffy");
			foreach (GameObject stuffy in possibleStuffies) {
				if (stuffy.transform.parent == null) { // Stuffy is on the ground
					target  = stuffy.transform;
					return;
				}

				if (stuffy.transform.parent.CompareTag("StuffyPile")) { // Stuffy is in pile
					target = stuffy.transform;
					return;
				}
			}
		}
	}

    void Update()
    {
        if (target == null)
            return;
		UpdateTarget();
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		SetAnimationDirection(GetDirectionToTarget());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile")) {

            // Remove bullet
            collision.gameObject.SetActive(false);
            // Spawn explosition
            ShootController sc = player.GetComponent<ShootController>();
            sc.SpawnExplosion(gameObject.transform.position);
            hp--;
			if (hp <= 0) {
				// Drop stuffy if any (detach from parent enemy)
				transform.DetachChildren();
                Destroy(gameObject);
			}
        }
        else if (collision.gameObject.CompareTag("Explosion")) {
            hp--;
            if (hp <= 0) {
                // Drop stuffy if any (detach from parent enemy)
                transform.DetachChildren();
                Destroy(gameObject);
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

	public void SetIsHoldingItem(bool holding) {
		isHolding = holding;
		UpdateTarget();

	}

	public bool IsHoldingItem() {
		return isHolding;
	}
}
