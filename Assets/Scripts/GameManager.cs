using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool isStopped;
    public BaseScript baseGO;
    public Text countOfEnemiesText;
    public int levelId;
    private EnemySpawnManager enemySpawnManager;
    private PlayerController playerController;

    private void Start()
    {
        enemySpawnManager = GetComponent<EnemySpawnManager>();
        enemySpawnManager.spawnFinished += UpdateCountOfEnemies;
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.playerDestroyed += PlayerController_playerDestroyed;
        baseGO = GameObject.Find("Base").GetComponent<BaseScript>();
        baseGO.baseDestroyed += GameManager_baseDestroyed;
    }

    private void UpdateCountOfEnemies(object sender, int e)
    {
        countOfEnemiesText.text = "Count of enemies: " + e;
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
                    enemy.GetComponent<Enemy>().DestroyTank();
                    Debug.Log("Enemy destoy");
                }
                break;
            case BonusType.boat:
                playerController.boat.SetActive(true);
                playerController.canMoveOnWater = true;
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
            enemy.GetComponent<Enemy>().canShoot = false;
        }
        yield return new WaitForSeconds(15f);

        isStopped = false;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().speed = 5;
            enemy.GetComponent<Enemy>().canShoot = true;
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
        int countEnemiesOnScene = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(enemySpawnManager.GetCountEnemiesToSpawn() == 0 && countEnemiesOnScene == 0)
        {
            SceneManager.LoadScene(levelId);
        }
    }
}
