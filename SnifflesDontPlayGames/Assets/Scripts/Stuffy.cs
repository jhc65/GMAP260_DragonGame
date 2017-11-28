using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuffy : MonoBehaviour {

	private Transform newParent;
    private Vector3 position;
	private GameObject objHolding;
	private Transform pile;
	private SpriteRenderer r;

    void Start() {
        position = transform.position;
		pile = transform.parent.transform;
		r = GetComponent<SpriteRenderer>();
		r.enabled = false; // invisible until stolen
    }

    void OnTriggerEnter2D(Collider2D collision) {

		// If a thief touches the stuffy and it has a parent (is a part of the stuffy pile), hold it
		if (collision.gameObject.CompareTag("Stealer")) {
			r.enabled = true; // stolen, make visible
			if (collision.gameObject.transform.childCount > 0) 
				return;
            newParent = collision.transform;
            transform.parent = newParent; //changes stuffy parent
			objHolding = collision.gameObject;
			objHolding.GetComponent<MoveTowardStuffies>().SetIsHoldingItem(true);
		}

		// If a player touches the stuffy and it has no parent (alone :( ), return it
		if (collision.gameObject.CompareTag("Player") && transform.parent == null) {
			ReturnStuffy();
		}
	}

	public void ReturnStuffy() {
		transform.position = position; //return to original position
		transform.parent = pile;
		gameObject.tag = "Stuffy";
		r.enabled = false;

	}

		
}
