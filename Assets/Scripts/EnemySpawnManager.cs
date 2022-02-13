using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawnManager : MonoBehaviour
{
    public List<GameObject> enemiesPrefabs;
    public List<Transform> spawnTransforms;
    public int needToKill;
    public float spawnTime;
    public event EventHandler<int> spawnFinished;
    public List<Rigidbody2D> spawnRigidbodies;
    private GameManager gameManager;
    
    private void Awake()
    {
        gameManager = Camera.main.GetComponent<GameManager>();

        StartCoroutine(WaitForSpawnEnemy());
    }

    void Update()
    { }

    IEnumerator WaitForSpawnEnemy()
    {
        while (needToKill > 0)
        {
            int prefabIndex = UnityEngine.Random.Range(0, enemiesPrefabs.Count);
            int spawnIndex = UnityEngine.Random.Range(0, spawnTransforms.Count);

            Transform spawnPosition = spawnTransforms[spawnIndex];
            Rigidbody2D spawnVisualition = spawnRigidbodies[spawnIndex];

            spawnVisualition.gameObject.SetActive(true);
            spawnVisualition.AddTorque(200f);
            spawnFinished?.Invoke(this, needToKill);

            yield return new WaitForSeconds(1.5f);

            spawnVisualition.gameObject.SetActive(false);
            GameObject enemy = Instantiate(enemiesPrefabs[prefabIndex], spawnPosition.position, spawnPosition.rotation);

            // Think about this duplicate.
            if (gameManager.isStopped)
            {
                enemy.GetComponent<Enemy>().canShoot = false;
                enemy.GetComponent<Enemy>().speed = 0;
            }
            else
            {
                enemy.GetComponent<Enemy>().canShoot = true;
                enemy.GetComponent<Enemy>().speed = 5;
            }

            needToKill--;
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public int GetCountEnemiesToSpawn()
    {
        return needToKill;
    }
}
