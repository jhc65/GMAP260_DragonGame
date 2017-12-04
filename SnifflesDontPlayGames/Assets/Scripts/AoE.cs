using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TriggerExplosionDamage(float radius)
	{

		// Find all the colliders on the Enemies layer within the expRadius.
		Collider2D[] enemies = Physics2D.OverlapCircleAll (transform.position, radius, 1 << LayerMask.NameToLayer("EnemyLayer"));

		// For each collider...
		foreach(Collider2D col in enemies)
		{
			if (col.CompareTag("Enemy")) {
				col.GetComponent<MoveTowardsPlayer>().Die();
			} else if (col.CompareTag("Stealer")) {
				col.GetComponent<MoveTowardStuffies>().Die();
			}
			// Check if it has a rigidbody
			/*
			Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
			if(rb != null && rb.CompareTag("Enemy")) {
				rb.GetComponent<MoveTowardsPlayer>().Die();
			} else if (rb != null && rb.CompareTag("Stealer")) {
				rb.GetComponent<MoveTowardStuffies>().Die();
			}
			*/
		}
	}

}
