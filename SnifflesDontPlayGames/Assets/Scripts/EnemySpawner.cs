using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Debugger Controls
	You can expand the spawnPoints parameter, and change the Size to the number
	  of desired spawn points.  For each point, enter the coordinates for spawn positions
	
	Change the spawnCooldown to the desired time between enemy spawns
*/
public class EnemySpawner : MonoBehaviour {

	public GameObject enemy;
	public Vector3[] spawnPoints;
	public float frequency = 5.0F;

	private float spawnCooldown = 0.0f;
	private bool enabled = true;

	void Start () {
		
	}
	
	void Update () {
		if (!enabled)
			return;
		
		if (spawnCooldown <= 0.0f && spawnPoints.Length > 0) {
			int i = Random.Range(0, spawnPoints.Length); // random spawn point
			Vector3 spawnPosition = spawnPoints[i];
			GameObject spawnedEnemy = GameObject.Instantiate(enemy);
			spawnedEnemy.transform.position = spawnPosition;
			spawnCooldown = frequency;
		}
		else {
			spawnCooldown -= Time.deltaTime;
		}
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false; 
	}
}
