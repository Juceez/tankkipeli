using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class GameControlScript : MonoBehaviour
{

    public SpawnerScript spawner;

    public float score;
    public float scorePerTank;
    public int lives;
    public int enemyStartAmount;
    public int maxEnemiesAmount;

    private int currentLives;
    private int currentEnemyAmount;

    private GameObject player;
    public static GameControlScript instance;
    public UIControl ui;
    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemyStartAmount; i++)
        {
            spawner.SpawnEnemy();
        }

        player = spawner.SpawnPlayer();

        score = 0f;
        currentLives = lives;
        currentEnemyAmount = enemyStartAmount;
        ui.SetScore(score);
        ui.SetLives(currentLives, lives);
     
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            if (currentLives > 0)
            {
                ui.showRespawn();
                if(Input.GetButton("Restart"))
                {
                    audioSource.Play();
                    player = spawner.SpawnPlayer();
                    currentLives--;
                    ui.SetLives(currentLives, lives);
                    ui.hideRespawn();
                }
            }
            else
            {
                ui.ShowEndScreen(score);
            }
        }
    }

    public void EnemyDestroyed()
    {
        spawner.SpawnEnemy();
        score += scorePerTank;
        ui.SetScore(score);
        if (currentEnemyAmount < maxEnemiesAmount)
        {
            spawner.SpawnEnemy();
            currentEnemyAmount++;
        }
    }

    public void SetHealth(float currentHP, float maxHP)
    {
        if (currentHP < 0) currentHP = 0f;
        ui.SetHealth(currentHP, maxHP);
    }
    
    
}
