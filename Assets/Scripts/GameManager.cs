using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] enemySpawnPositions;
    public int countOfEnemy = 10;
    public int countOfEnemyOnScene;
    public int needToKill;

    [SerializeField] private float spawnTime = 5f;
    private float currentSpawnTime;
    public BaseScript baseGO;
    public Text countOfEnemiesText;

    private PlayerController playerController;

    private List<GameObject> enemies;
    private void Start()
    {
        needToKill = countOfEnemy;

        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.playerDestroyed += PlayerController_playerDestroyed;
        baseGO = GameObject.Find("Base").GetComponent<BaseScript>();
        baseGO.baseDestroyed += GameManager_baseDestroyed;
    }

    private void PlayerController_playerDestroyed(object sender, GameObject e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameManager_baseDestroyed(object sender, GameObject baseGO)
    {
        Destroy(baseGO);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Update()
    {

        currentSpawnTime += Time.deltaTime;

        if (currentSpawnTime >= spawnTime && countOfEnemy > 0)
        {
            SpawnEnemy();
            currentSpawnTime = 0;
        }
        if(countOfEnemyOnScene == 0 && needToKill <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        countOfEnemiesText.text = "Count of enemies: " + needToKill;
    }

    void SpawnEnemy()
    {
        countOfEnemy--;
        Transform enemySpawnPosition = enemySpawnPositions[UnityEngine.Random.Range(0, enemySpawnPositions.Length)];
        GameObject enemy = Instantiate(enemyPrefab, enemySpawnPosition.position, enemySpawnPosition.rotation);
        enemy.GetComponent<Enemy>().enemyDestroyed += GameManager_enemyDestroyed;
        countOfEnemyOnScene++;
    }

    private void GameManager_enemyDestroyed(object sender, GameObject e)
    {
        countOfEnemyOnScene--;
        needToKill--;
    }
}
