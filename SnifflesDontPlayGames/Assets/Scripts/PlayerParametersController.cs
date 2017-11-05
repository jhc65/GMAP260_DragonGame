using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerParametersController : MonoBehaviour
{
    // Public Vars
    // Controllers (probably set in editor this time? No instances to pull from in this Playable)
    public GameController gameController;
    public GameObject DevUI;
    public PlayerController snifflesTheDragon;
    public ShootController shootController;
    public List<MoveTowardsPlayer> firstFourEnemies = new List<MoveTowardsPlayer>(4);
    public MoveTowardsPlayer enemyAI_Prefab;

    // UI Sliders (Set in editor)
    public Slider playerHealth;
    public Slider playerMoveSpeed;
    public Slider playerAttackSpeed;
    public Slider playerStuffyCount;
    public Slider enemySpawnTimer;
    public Slider warriorMovementSpeed;
    public Slider rogueMovementSpeed;
    public Slider enemyHealth_hitsToKill;

    // UI Text (Set in editor)
    public Text playerHealthValue;
    public Text playerMoveSpeedValue;
    public Text playerAttackSpeedValue;
    public Text playerStuffyCountValue;
    public Text enemySpawnTimerValue;
    public Text warriorMovementSpeedValue;
    public Text rogueMovementSpeedValue;
    public Text enemyHealth_hitsToKillValue;

    private void Start()
    {
        SetPlayerHealthSlider();
        SetPlayerMoveSpeedSlider();
        SetEnemySpawnTimerSlider();
        SetEnemyHealthSlider();
        SetWarriorMoveSpeed();
        SetRogueMoveSpeed();
    }

    // Update is called once per frame
    private void Update()
    {

    }
    
    // Initial Setters for sliders, incase we change base values in code
    private void SetPlayerHealthSlider()
    {
        float playerHP = snifflesTheDragon.GetPlayerHealth();
        playerHealth.value = (float)playerHP;
        playerHealthValue.text = playerHP.ToString();
    }

    private void SetPlayerMoveSpeedSlider()
    {
        float playerSpeed = snifflesTheDragon.GetPlayerMovementSpeed();
        playerMoveSpeed.value = playerSpeed;
        playerMoveSpeedValue.text = playerSpeed.ToString();
    }

    private void SetEnemySpawnTimerSlider()
    {
        float enemySpawnFrequency = gameController.GetEnemySpawnFrequency();
        enemySpawnTimer.value = enemySpawnFrequency;
        enemySpawnTimerValue.text = enemySpawnFrequency.ToString();
    }

    private void SetEnemyHealthSlider()
    {
        float enemyHealth = enemyAI_Prefab.GetEnemyHealth();
        enemyHealth_hitsToKill.value = enemyHealth;
        enemyHealth_hitsToKillValue.text = enemyHealth.ToString();
    }

    private void SetWarriorMoveSpeed()
    {
        float warriorSpeed = enemyAI_Prefab.GetEnemyMoveSpeed();
        warriorMovementSpeed.value = warriorSpeed;
        warriorMovementSpeedValue.text = warriorSpeed.ToString();
    }

    private void SetRogueMoveSpeed()
    {
        float rogueSpeed = enemyAI_Prefab.GetEnemyMoveSpeed();
        rogueMovementSpeed.value = rogueSpeed;
        rogueMovementSpeedValue.text = rogueSpeed.ToString();
    }

    // Public methods to actually change the values
    public void ChangePlayerHealth(float newHealthIn)
    {
        playerHealthValue.text = ((int)newHealthIn).ToString();
        snifflesTheDragon.ChangePlayerHealth((int)newHealthIn);
    }

    public void ChangePlayerMovementSpeed(float newMoveSpeedIn)
    {
        playerMoveSpeedValue.text = newMoveSpeedIn.ToString();
        snifflesTheDragon.ChangePlayerMovementSpeed(newMoveSpeedIn);
    }

    public void ChangeEnemySpawnFrequency(float newFrequencyIn)
    {
        enemySpawnTimerValue.text = newFrequencyIn.ToString();
        gameController.ChangeEnemySpawnFrequency(newFrequencyIn);
    }

    public void ChangeEnemyHealth(float newHealthIn)
    {
        enemyHealth_hitsToKillValue.text = ((int)newHealthIn).ToString();
        foreach (MoveTowardsPlayer enemy in firstFourEnemies) {
            enemy.ChangeEnemyHealth((int)newHealthIn);
        }

        enemyAI_Prefab.ChangeEnemyHealth((int)newHealthIn);
    }

    public void ChangeWarriorMovementSpeed(float newMoveSpeedIn)
    {
        warriorMovementSpeedValue.text = newMoveSpeedIn.ToString();
        rogueMovementSpeedValue.text = newMoveSpeedIn.ToString();
        rogueMovementSpeed.value = newMoveSpeedIn;
        foreach (MoveTowardsPlayer enemy in firstFourEnemies) {
            enemy.ChangeEnemyMoveSpeed(newMoveSpeedIn);
        }

        enemyAI_Prefab.ChangeEnemyMoveSpeed(newMoveSpeedIn);
    }

    public void ChangeRogueMovementSpeed(float newMoveSpeedIn)
    {
        warriorMovementSpeedValue.text = newMoveSpeedIn.ToString();
        warriorMovementSpeed.value = newMoveSpeedIn;
        rogueMovementSpeedValue.text = newMoveSpeedIn.ToString();
        foreach (MoveTowardsPlayer enemy in firstFourEnemies) {
            enemy.ChangeEnemyMoveSpeed(newMoveSpeedIn);
        }

        enemyAI_Prefab.ChangeEnemyMoveSpeed(newMoveSpeedIn);
    }

    // Changes whether the Parameter UI is visible, and "pauses" the game if it is
    public void ToggleDevUI()
    {
        if (DevUI.activeSelf) {
            DevUI.SetActive(false);
            gameController.SetTimeScale(1);
        }
        else {
            gameController.SetTimeScale(0);
            DevUI.SetActive(true);
        }
    }
}
