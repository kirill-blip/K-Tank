using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] enemySpawnPositions;
    public List<Rigidbody2D> enemyRigigbodys;
    public int countOfEnemy = 10;
    public int countOfEnemyOnScene;
    public int needToKill;

    [SerializeField] private float spawnTime = 3f;
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
        GameObject enemy = Instantiate(enemyPrefab, enemyPos.position, enemyPos.rotation);
        enemy.GetComponent<Enemy>().enemyDestroyed += GameManager_enemyDestroyed;
    }
    private void GameManager_enemyDestroyed(object sender, GameObject e)
    {
        countOfEnemyOnScene--;
        needToKill--;
    }
}
