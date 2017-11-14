﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StuffyWatcher : MonoBehaviour {

	public Text stuffyCount;
	public GameObject player; // Use for game over call (Bad place for it atm)

	private int numStuffiesTotal;
	private int numStuffiesInPile;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		numStuffiesInPile = GameObject.FindGameObjectsWithTag("Stuffy").Length;

		numStuffiesTotal = numStuffiesInPile +
			GameObject.FindGameObjectsWithTag("Held").Length + 
			GameObject.FindGameObjectsWithTag("Dropped").Length;
		PrintStuffyCount();

	}

	void PrintStuffyCount() {
		if (numStuffiesTotal == 0) {
			stuffyCount.text = "Game Over";
			player.GetComponent<PlayerController>().GameOver();
			return;
		}
		stuffyCount.text = "Stuffies In Castle: " + numStuffiesTotal;
		
	}
}