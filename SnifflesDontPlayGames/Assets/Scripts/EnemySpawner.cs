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

    public GameController gameController;

	public GameObject enemy;
	public Vector3[] spawnPoints;
	public float frequency = 5.0F;
	public int maxToSpawn = 4;
	public float spreadDistance = 20.0f;

	private float spawnCooldown = 0.0f;
	private bool enabled = true;

	void Start () {
		gameController.ChangeEnemySpawnFrequency(frequency);
	}
	
	void Update () {
		if (!enabled)
			return;
		
		if (spawnCooldown <= 0.0f && spawnPoints.Length > 0) {
			int numToSpawn = Random.Range(1, maxToSpawn);
			int spawnPointIndex = Random.Range(0, spawnPoints.Length); // random spawn point
			
			for (int i = 0; i < numToSpawn; i++)
			{
				Vector3 spawnPosition = spawnPoints[spawnPointIndex];
				GameObject spawnedEnemy = GameObject.Instantiate(enemy);
				Vector3 positionToMove = spawnPosition;
				switch (i % 5)
				{
					case 1:
						positionToMove += spreadDistance * Vector3.up;
						break;
					case 2:
						positionToMove += spreadDistance * Vector3.right;
						break;
					case 3:
						positionToMove += spreadDistance * Vector3.down;
						break;
					case 4:
						positionToMove += spreadDistance * Vector3.left;
						break;
						
					default:

						break;
				}
				spawnedEnemy.transform.position = positionToMove;
				spawnCooldown = gameController.GetEnemySpawnFrequency();
			}
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
