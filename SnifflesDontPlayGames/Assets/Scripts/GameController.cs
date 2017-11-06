using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject DevUI;
    public PlayerParametersController devUIController;
    private float enemySpawnFrequency = .01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
            devUIController.ToggleDevUI();
        }
	}

    public void SetTimeScale(int timeScaleIn)
    {
        Time.timeScale = timeScaleIn;
    }

    public float GetEnemySpawnFrequency()
    {
        return enemySpawnFrequency;
    }

    public void ChangeEnemySpawnFrequency(float newSpawnFreqencyIn)
    {
        enemySpawnFrequency = newSpawnFreqencyIn;
    }
}
