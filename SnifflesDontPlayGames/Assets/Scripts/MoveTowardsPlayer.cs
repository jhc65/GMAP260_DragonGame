﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour {

    private Transform target;
    public float speed = 5f;
	private int hp = 2;
	private Color hitColor = new Color(255f,255f,255f);
		
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Projectile") {
			
			// Remove bullet
			Destroy(collision.collider.gameObject);

			hp--;
			GetComponent<Renderer>().material.color = hitColor; // Effect to show enemy was hit. Change color to white
			if (hp <= 0)
				Destroy(gameObject);
				
		}
	}
}
