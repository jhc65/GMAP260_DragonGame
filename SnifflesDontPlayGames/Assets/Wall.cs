using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag("Player")) {
			print("Wall hit");
			col.gameObject.GetComponent<PlayerController>().DisableMovement();

		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.CompareTag("Player")) {
			print("Wall released");
			col.gameObject.GetComponent<PlayerController>().EnableMovement();

		}
	}


}
	
