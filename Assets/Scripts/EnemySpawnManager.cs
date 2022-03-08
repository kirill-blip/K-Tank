using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawnManager : MonoBehaviour
{
    public List<GameObject> enemiesPrefabs;
    public List<Transform> spawnTransforms;
    public List<Rigidbody2D> spawnRigidbodies;

    public int needToKill;
    public float spawnTime;
    public event EventHandler<int> spawnFinished;

    private int currentEnemyOnScene;
    private const int enemyOnScene = 4;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = Camera.main.GetComponent<GameManager>();
        StartCoroutine(WaitForSpawnEnemy());
    }
    private IEnumerator WaitForSpawnEnemy()
    {
        while (needToKill > 0)
        {
            if(currentEnemyOnScene == enemyOnScene)
            {
                yield return new WaitForSeconds(spawnTime);
                continue;
            }
            int prefabIndex = UnityEngine.Random.Range(0, enemiesPrefabs.Count);
            int spawnIndex = UnityEngine.Random.Range(0, spawnTransforms.Count);

            Transform spawnPosition = spawnTransforms[spawnIndex];
            Rigidbody2D spawnVisualition = spawnRigidbodies[spawnIndex];

            spawnVisualition.gameObject.SetActive(true);
            spawnVisualition.AddTorque(200f);
            spawnFinished?.Invoke(this, needToKill);
            currentEnemyOnScene++;

            yield return new WaitForSeconds(1.5f);
            spawnVisualition.gameObject.SetActive(false);
            GameObject enemyGO = Instantiate(enemiesPrefabs[prefabIndex], spawnPosition.position, spawnPosition.rotation);
            Enemy enemyScipt = enemyGO.GetComponent<Enemy>();
            enemyScipt.enemyDestroyed += EnemySpawnManager_enemyDestroyed;
            enemyScipt.DefaultRotationAndPosition();

            if (gameManager.isStopped)
                enemyScipt.StopTank();
            else
                enemyScipt.StartTank();

            needToKill--;
            yield return new WaitForSeconds(spawnTime);
        }
    }
    private void EnemySpawnManager_enemyDestroyed(object sender, GameObject e)
    {
        currentEnemyOnScene--;
    }
    public int GetCountEnemiesToSpawn()
    {
        return needToKill;
    }
}
