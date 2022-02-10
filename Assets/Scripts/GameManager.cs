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
    public Text healthText;
    public int levelId;
    public Transform playerPoint;
    public Rigidbody2D playerPointRigid;
    private EnemySpawnManager enemySpawnManager;
    private PlayerController playerController;

    private void Start()
    {
        playerPointRigid = playerPoint.GetComponentInChildren<Rigidbody2D>();
        playerPointRigid.gameObject.SetActive(false);

        enemySpawnManager = GetComponent<EnemySpawnManager>();
        enemySpawnManager.spawnFinished += UpdateCountOfEnemies;
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.playerDestroyed += PlayerController_playerDestroyed;
        baseGO = GameObject.Find("Base").GetComponent<BaseScript>();
        baseGO.baseDestroyed += GameManager_baseDestroyed;
        healthText.text = "Health: " + playerController.GetHealth();
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
                playerController.turboShooting = true;
                //StartCoroutine(ChangeShootingTime());
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
        if (playerController.GetHealth() == 0)
        {
            StartCoroutine(LoadSceneInTime());
            return;
        }
        healthText.text = "Health: " + playerController.GetHealth();
        StartCoroutine(SetActivePlayer());
    }
    IEnumerator SetActivePlayer()
    {
        playerPointRigid.gameObject.SetActive(true);
        playerPointRigid.AddTorque(200f);
        yield return new WaitForSeconds(.5f);

        playerController.particleSystem.Stop();
        playerController.gameObject.SetActive(false);
        playerController.transform.position = playerPoint.position;
        playerController.movePoint.position = playerPoint.position;
        yield return new WaitForSeconds(1.5f);

        playerController.GetComponent<Collider2D>().enabled = true;
        playerController.gameObject.SetActive(true);
        playerController.canMove = true;
        playerPointRigid.gameObject.SetActive(false);
    }

    IEnumerator LoadSceneInTime()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }

    private void GameManager_baseDestroyed(object sender, GameObject baseGO)
    {
        Destroy(baseGO);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        int countEnemiesOnScene = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemySpawnManager.GetCountEnemiesToSpawn() == 0 && countEnemiesOnScene == 0)
        {
            SceneManager.LoadScene(levelId);
        }
    }
}
