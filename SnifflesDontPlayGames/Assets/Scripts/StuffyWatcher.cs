using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StuffyWatcher : MonoBehaviour {

	public Text stuffyCount;
	public GameObject player; // Use for game over call (Bad place for it atm)
	public Sprite[] pileSprites;

	private int numStuffiesTotal;
	private int numStuffiesStolen = 0;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		UpdateStuffyCount();
	}

	void UpdateStuffyCount() {
		GameObject[] stuffies = GameObject.FindGameObjectsWithTag("Stuffy");

		// Count all stuffies that are children of the stuffy pile
		// Horribly not efficient but too lazy to do this another way
		foreach (GameObject stuff in stuffies) {
			if (stuff.transform.parent != null && stuff.transform.parent.CompareTag("StuffyPile")) 
				numStuffiesStolen++;
		}
		numStuffiesTotal = stuffies.Length;
		UpdateStuffySprite();
		if (numStuffiesTotal == 0) {
			stuffyCount.text = "Game Over";
			player.GetComponent<PlayerController>().GameOver();
			return;
		}
		stuffyCount.text = "Stuffies In Castle: " + numStuffiesTotal;
		numStuffiesStolen = 0;

	}

	void UpdateStuffySprite() {
		GetComponent<SpriteRenderer>().sprite = pileSprites[numStuffiesStolen];

	}
}
