using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float playerSpeed = 4f;

	
	void Start () {
		
	}
	

	void FixedUpdate () {

        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        GetComponent<Rigidbody2D>().velocity = targetVelocity * playerSpeed;
    }
}
