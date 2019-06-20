using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Game")]
    public Player player;
    public GameObject enemyContainer;

    [Header("UI")]
    public Text ammoText;
    public Text healthText;
    public Text enemyText;
    public Text infoText;
    public Text waveText;

    [Header("Enemies")]
    public int wave = 0;
    public int enemies;
    public int aliveEnemies = 0;
    public GameObject EnemyPrefab;
    public GameObject[] spawnLocations;

    private bool gameOver = false;
    private float resetTimer = 3.0f;
    

    private void Start()
    {
        enemies = 2;
        infoText.gameObject.SetActive(false);
    }

    private void NewWave()
    {
        enemies = enemies * wave;
        for(int s = 0; s < spawnLocations.Length; s++)
        {
            for(int i = 0; i < enemies; i++)
            {
                Instantiate(EnemyPrefab, spawnLocations[s].transform.position, Quaternion.identity, spawnLocations[s].transform);
            }
        }

        foreach (Enemy enemy in enemyContainer.GetComponentsInChildren<Enemy>())
        {
            if (enemy.Killed == false)
            {
                aliveEnemies++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.Health;
        ammoText.text = "Ammo: " + player.Ammo;
        enemyText.text = "Enemies: " + aliveEnemies;
        waveText.text = "Wave: " + wave;


        if (aliveEnemies == 0)
        {
            wave++;
            NewWave();
        }

        if (player.Killed == true)
        {
            gameOver = true;
            infoText.gameObject.SetActive(true);
            infoText.text = "You Lose\nTry Again";
        }

        if(gameOver == true)
        {
            resetTimer -= Time.deltaTime;
            if(resetTimer <= 0)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
