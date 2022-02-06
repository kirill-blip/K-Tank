using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public Transform[] enemySpawnPositions;
    public List<Rigidbody2D> enemyRigigbodys;
    public int countOfEnemy = 10;
    public int countOfEnemyOnScene;
    public int needToKill;
    bool isStopped;
    [SerializeField] private float spawnTime = 3f;
    private float currentSpawnTime;
    public BaseScript baseGO;
    public Text countOfEnemiesText;

    private PlayerController playerController;

    private void Start()
    {
        needToKill = countOfEnemy;

        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.playerDestroyed += PlayerController_playerDestroyed;
        baseGO = GameObject.Find("Base").GetComponent<BaseScript>();
        baseGO.baseDestroyed += GameManager_baseDestroyed;
    }

    public void GameManager_onBonus(object sender, BonusType type)
    {
        switch (type)
        {
            case BonusType.shootingTime:
                StartCoroutine(ChangeShootingTime());
                break;
            case BonusType.stopTimeForEnemy:
                StartCoroutine(StoppingEnemy());
                break;
            case BonusType.bomb:
                var enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var enemy in enemies)
                {
                    Destroy(enemy);
                    countOfEnemyOnScene--;
                    needToKill--;
                    Debug.Log("Enemy destoy");
                }
                break;
            default:
                break;
        }
    }
    IEnumerator StoppingEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        isStopped = true;
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().speed = 0;
        }
        yield return new WaitForSeconds(15f);
        isStopped = false;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().speed = 5;
        }
    }
    float temp;
    IEnumerator ChangeShootingTime()
    {
        if (!playerController.timeOfShootingChanged)
        {
            temp = playerController.maxShootingTime;
            playerController.maxShootingTime /= 2;
            playerController.timeOfShootingChanged = true;
        }
        yield return new WaitForSeconds(15f);
        if (playerController.timeOfShootingChanged)
        {
            playerController.maxShootingTime = temp;
            playerController.timeOfShootingChanged = false;
        }

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
        if (countOfEnemyOnScene == 0 && needToKill <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        countOfEnemiesText.text = "Count of enemies: " + needToKill;
    }

    void SpawnEnemy()
    {
        countOfEnemy--;
        int index = UnityEngine.Random.Range(0, enemySpawnPositions.Length);
        Transform enemySpawnPosition = enemySpawnPositions[index];
        Rigidbody2D rigid = enemyRigigbodys[index];
        rigid.gameObject.SetActive(true);
        rigid.AddTorque(100);

        StartCoroutine(WaitForSpawn(enemySpawnPosition, index));
        countOfEnemyOnScene++;
    }
    IEnumerator WaitForSpawn(Transform enemyPos, int index)
    {
        yield return new WaitForSeconds(1.5f);
        enemyRigigbodys[index].gameObject.SetActive(false);

        int randomEnemyIndex = UnityEngine.Random.Range(0, enemyPrefabs.Count);

        GameObject enemy = Instantiate(enemyPrefabs[randomEnemyIndex], enemyPos.position, enemyPos.rotation);
        enemy.GetComponent<Enemy>().enemyDestroyed += GameManager_enemyDestroyed;
        if (isStopped)
            enemy.GetComponent<Enemy>().speed = 0;
        else if (!isStopped)
            enemy.GetComponent<Enemy>().speed = 5;

    }
    private void GameManager_enemyDestroyed(object sender, GameObject e)
    {
        countOfEnemyOnScene--;
        needToKill--;
    }
}
