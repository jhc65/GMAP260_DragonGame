using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuffy : MonoBehaviour {

	private Transform newParent;
    private Vector3 position;

	private bool isHeld = false;
	private GameObject objHolding;

    void Start() {
        position = transform.position;

    }

    void OnTriggerEnter2D(Collider2D collision) {

		if (collision.gameObject.CompareTag("Stealer") && !isHeld)
        {
            newParent = collision.transform;
            transform.parent = newParent; //changes stuffy parent
			objHolding = collision.gameObject;
			SetHold(true);
		}
			
	}

	public void ReturnStuffy() {
		transform.position = position; //return to original position
		isHeld = false;
		gameObject.tag = "Stuffy";
	}


	public void SetHold(bool isholding) {
		isHeld = isholding;
		if (!isHeld) {
			gameObject.tag = "Dropped";
		} else {
			gameObject.tag = "Held";
		}
			objHolding.GetComponent<MoveTowardStuffies>().SetIsHoldingItem(isHeld);
	}

		
}
