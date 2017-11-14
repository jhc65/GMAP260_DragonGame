using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefLeaving : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		// Stealer leaves the map when touching the door
		if (collision.gameObject.CompareTag("Stealer"))
		{
			collision.gameObject.SetActive(false);
		}
	}
}
