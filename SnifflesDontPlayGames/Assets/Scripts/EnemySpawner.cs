using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	INSTRUCTIONS TO USE

	Attach this script to any game object in the scene, preferably
	  an empty game object called EnemySpawner or similar

	Click on the game object, and drag the enemy prefab onto the
	  ENEMY_TO_SPAWN parameter
	
	Expand the SPAWN_POINTS parameter, changing the Size to the number
	  of desired spawn points.  For each point, enter the coordinates
	
	Change the SPAWN_FREQUENCY to the desired time between enemy spawns
*/
public class EnemySpawner : MonoBehaviour {

	public GameObject ENEMY_TO_SPAWN;
	public Vector3[] SPAWN_POINTS;
	public float SPAWN_FREQUENCY = 5.0F;

	private float _timeUntilNextSpawn = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_timeUntilNextSpawn <= 0.0f && SPAWN_POINTS.Length > 0)
		{
			int i = Random.Range(0, SPAWN_POINTS.Length);
			Vector3 spawnPosition = SPAWN_POINTS[i];
			GameObject spawnedEnemy = GameObject.Instantiate(ENEMY_TO_SPAWN);
			spawnedEnemy.transform.position = spawnPosition;
			_timeUntilNextSpawn = SPAWN_FREQUENCY;
		}
		else
		{
			_timeUntilNextSpawn -= Time.deltaTime;
		}
	}
}
