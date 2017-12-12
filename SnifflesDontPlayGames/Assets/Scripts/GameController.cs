using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Text scoreboard;
	public int score;
    public GameObject DevUI;
    public PlayerParametersController devUIController;
    private float enemySpawnFrequency = .01f;
	private bool gameIsRunning;

	// Use this for initialization
	void Start () {
		score = 0;
		gameIsRunning = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameIsRunning)
			return;

		// Increment score as time goes on
		score++;
		scoreboard.text = score.ToString();
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

	public void IncrementScoreByAmount(int amount) {
		score += amount;
		scoreboard.text = score.ToString();
	}

	public void StopScoring() {
		gameIsRunning = false;
	}
}
