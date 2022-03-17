using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class GameManager : MonoBehaviour
{
    public int levelId;
    public HomeScript baseGO;
    public Transform playerPoint;
    public Rigidbody2D playerPointRigid;
    public GameObject gameOverPanel;

    public bool isStopped;
    public bool canLoadLevel = false;

    private EnemySpawnManager enemySpawnManager;
    private PlayerController playerController;
    private DataManager dataManager;
    private LevelManager levelManager;

    public event EventHandler<bool> loadFirstLevel;

    private void Start()
    {
        dataManager = GameObject.FindObjectOfType<DataManager>();
        levelManager = GameObject.FindObjectOfType<LevelManager>();

        enemySpawnManager = GetComponent<EnemySpawnManager>();

        playerPointRigid = playerPoint.GetComponentInChildren<Rigidbody2D>();
        playerPointRigid.gameObject.SetActive(false);

        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.playerDamaged += PlayerController_playerDestroyed;

        baseGO = GameObject.Find("Base").GetComponent<HomeScript>();
        baseGO.homeDestroyed += GameManager_baseDestroyed;
    }
    private void Update()
    {
        //if (canLoadLevel)
        //{
        //    if (Input.anyKey)
        //        StartCoroutine(levelManager.WaitForLoadScene(1, eSceneType.menuScene));
        //}

        //if (enemySpawnManager.GetCountEnemiesToSpawn() <= 0 && enemySpawnManager.GetCurrentEnemyOnScene() == 0)
        //{
        //    if (levelId != 0)
        //    {
        //        dataManager.SavePlayerData(playerController);
        //    }
        //    StartCoroutine(levelManager.WaitForLoadScene(5f, eSceneType.levelScene));
        //}
    }
    private void PlayerController_playerDestroyed(object sender, GameObject e)
    {
        if (playerController.GetHealth() == 0)
        {
            gameOverPanel.SetActive(true);
            loadFirstLevel?.Invoke(this, true);
            return;
        }
        StartCoroutine(SetActivePlayer());
    }
    private void GameManager_baseDestroyed(object sender, GameObject baseGO)
    {
        Destroy(baseGO);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private IEnumerator SetActivePlayer()
    {
        playerPointRigid.gameObject.SetActive(true);
        playerPointRigid.AddTorque(200f);
        yield return new WaitForSeconds(.5f);

        playerController.playerParticleSystem.Stop();
        playerController.gameObject.SetActive(false);
        playerController.transform.position = playerPoint.position;
        playerController.movePoint.position = playerPoint.position;
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(playerController.SetActiveShield(5f));
        playerController.GetComponent<Collider2D>().enabled = true;
        playerController.gameObject.SetActive(true);
        playerController.canMove = true;
        playerPointRigid.gameObject.SetActive(false);
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }
}