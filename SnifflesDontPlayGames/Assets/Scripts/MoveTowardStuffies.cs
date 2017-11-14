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
	void UpdateTarget() {
		if (isHolding) { // If holding stuffy head towards the door
			target = GameObject.FindGameObjectWithTag("Door").transform;
			return;
		} else { // if not holding stuffy, head towards stuffy pile 
			
			if (GameObject.FindGameObjectsWithTag("Stuffy").Length == 0) {
				stuffyObj = GameObject.FindGameObjectWithTag("Dropped");
			}
			else {
				stuffyObj = GameObject.FindGameObjectWithTag("Stuffy");

			}
			if (GameObject.FindGameObjectsWithTag("Stuffy").Length == 0 &&
				GameObject.FindGameObjectsWithTag("Dropped").Length == 0) {
				target = GameObject.FindGameObjectWithTag("Door").transform;
				return;
			}
			 
			target = stuffyObj.transform;

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
				if (isHolding)  { // If holding stuffy, drop it and let stuffy pile know
					stuffyObj.GetComponent<Stuffy>().SetHold(false);
					stuffyObj.tag = "Dropped";
					transform.DetachChildren();

				}
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
	}

	public bool IsHoldingItem() {
		return isHolding;
	}
}
