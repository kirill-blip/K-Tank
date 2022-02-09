using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
public class EnemySpawnManager : MonoBehaviour
{
    public List<GameObject> enemiesPrefabs;
    public List<Transform> spawnTransforms;
    public int needToKill;
    public float spawnTime;
    private float currentSpawnTime;
    private GameManager gameManager;
    public List<Rigidbody2D> spawnRigidbodies;

    public event EventHandler<int> spawnFinished;
    private void Awake()
    {
        gameManager = Camera.main.GetComponent<GameManager>();
    }

    void Update()
    {
        currentSpawnTime += Time.deltaTime;

        if (currentSpawnTime >= spawnTime && needToKill > 0)
        {
            StartCoroutine(WaitForSpawnEnemy());
            currentSpawnTime = 0;
        }
    }
    IEnumerator WaitForSpawnEnemy()
    {
        needToKill--;
        int randomIndex = UnityEngine.Random.Range(0, enemiesPrefabs.Count);
        int ind = UnityEngine.Random.Range(0, spawnTransforms.Count);
        Transform randPosition = spawnTransforms[ind];
        Rigidbody2D spawnVisualition = spawnRigidbodies[ind];
        spawnVisualition.gameObject.SetActive(true);
        spawnVisualition.AddTorque(200f);
        spawnFinished?.Invoke(this, needToKill);
        yield return new WaitForSeconds(1.5f);

        spawnVisualition.gameObject.SetActive(false);
        GameObject enemy = Instantiate(enemiesPrefabs[randomIndex], randPosition.position, randPosition.rotation);

        if (gameManager.isStopped)
        {

            enemy.GetComponent<Enemy>().canShoot = false;
            enemy.GetComponent<Enemy>().speed = 0;
        }
        else if (!gameManager.isStopped)
        {
            enemy.GetComponent<Enemy>().canShoot = true;
            enemy.GetComponent<Enemy>().speed = 5;
        }
    }
    public int GetCountEnemiesToSpawn()
    {
        return needToKill;
    }
}
