using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] enemySpawnPositions;
    public int countOfEnemy = 20;

    [SerializeField] private float spawnTime = 5f;
    private float currentSpawnTime;
    public BaseScript baseGO;

    private void Start()
    {
        baseGO = GameObject.Find("Base").GetComponent<BaseScript>();
        baseGO.baseDestroyed += GameManager_baseDestroyed;
    }

    private void GameManager_baseDestroyed(object sender, GameObject baseGO)
    {
        Destroy(baseGO);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public int countOfEnemyOnScene;
    private void Update()
    {
        countOfEnemyOnScene = GameObject.FindGameObjectsWithTag("Enemy").Length;
        currentSpawnTime += Time.deltaTime;

        if (currentSpawnTime >= spawnTime && countOfEnemy > 0)
        {
            SpawnEnemy();
            currentSpawnTime = 0;
        }
        if (countOfEnemy == 0 && countOfEnemyOnScene == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void SpawnEnemy()
    {
        countOfEnemy--;
        Transform enemySpawnPosition = enemySpawnPositions[Random.Range(0, enemySpawnPositions.Length)];
        GameObject enemy = Instantiate(enemyPrefab, enemySpawnPosition.position, enemySpawnPosition.rotation);
    }
}
