using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardStuffies : MonoBehaviour {

    public float speed = 5f;
    private int hp = 1;
    private FacingDir currentDir;
    private Transform target; // This is set to private since making it public would require prefabbing the player...and then the text...this is just easier for now
	private GameObject player;
    void Start()
    {
        currentDir = new FacingDir();
        currentDir = GetDirectionToTarget();
        target = GameObject.FindGameObjectWithTag("Stuffy").transform;
		player = GameObject.FindGameObjectWithTag("Player");
    }

    // Get the direction to the target.
    FacingDir GetDirectionToTarget()
    {
        FacingDir leftDir = new FacingDir("left");
        FacingDir rightDir = new FacingDir("right");

        if (target == null)
            return leftDir;

        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 toTarget = target.position - transform.position;
        float leftDotProduct = Vector3.Dot(left, toTarget);
        if (leftDotProduct < 0)
        { // target is to the right
            return rightDir;
        }
        else if (leftDotProduct > 0)
        { // target is to the right
            return leftDir;
        }
        else
        {
            // target reached stuffy - need to attach stuffy and change target

            //target = GameObject.FindGameObjectWithTag("Stuffy").transform;
            return leftDir; 
        }
            
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


    void Update()
    {
        if (target == null)
            return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        FacingDir flipTo = GetDirectionToTarget();
        flipTo.Flip();
        FlipDir(flipTo);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PolygonCollider2D>().tag == "Projectile")
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
