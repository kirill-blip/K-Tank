using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eSceneType
{
    menuScene,
    endScene,
    levelScene
}

public class LevelManager : MonoBehaviour
{
    private eSceneType sceneType;
    public bool loadLevelByAnyKey;

    private const int menuSceneId = 0;
    private const int endSceneId = 1;
    private const int firsLevelId = 2;
    private int sceneId;
    [SerializeField] private bool isLastLevel = false;
    private GameManager gameManager;
    private EnemySpawnManager enemySpawnManager;
    private bool canLoadFisrtLevel = false;
    private DataManager dataManager;

    // Start is called before the first frame update
    void Start()
    {
        try
        {

            enemySpawnManager = GameObject.FindObjectOfType<EnemySpawnManager>();
            gameManager = GameObject.FindObjectOfType<GameManager>();
            dataManager = GameObject.FindObjectOfType<DataManager>();
            gameManager.loadFirstLevel += GameManager_loadFirstLevel;
        }
        catch (Exception e)
        {
            Debug.Log("Something doesn't exist. " + e);
        }
    }

    private void GameManager_loadFirstLevel(object sender, bool canLoadScene)
    {
        canLoadFisrtLevel = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (loadLevelByAnyKey && Input.anyKeyDown)
        {
            SceneManager.LoadScene(menuSceneId);
        }

        if (canLoadFisrtLevel && Input.anyKeyDown)
        {
            SceneManager.LoadScene(firsLevelId);
        }

        if (enemySpawnManager != null && enemySpawnManager.GetCountEnemiesToSpawn() <= 0 && enemySpawnManager.GetCurrentEnemyOnScene() == 0)
        {
            if (CanSaveData())
            {
                dataManager.SavePlayerData();
            }
            StartCoroutine(WaitForLoadScene(5f, eSceneType.levelScene));
        }

    }

    private bool CanSaveData()
    {
        if (GetBuildSceneId() != menuSceneId && GetBuildSceneId() != endSceneId)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public IEnumerator WaitForLoadScene(float time, eSceneType type)
    {
        Time.timeScale = 1f;

        yield return new WaitForSeconds(time);

        if (isLastLevel)
            sceneType = eSceneType.endScene;
        else
            sceneType = type;

        LoadScene();
    }
    public IEnumerator LoadSceneByButton(int id, float time)
    {
        Time.timeScale = 1f;
        sceneId = id;

        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(id);
    }
    private void LoadScene()
    {
        switch (sceneType)
        {
            case eSceneType.menuScene:
                SceneManager.LoadScene(menuSceneId);
                break;
            case eSceneType.endScene:
                SceneManager.LoadScene(endSceneId);
                break;
            case eSceneType.levelScene:
                int nextLevelId = GetBuildSceneId() + 1;
                SceneManager.LoadScene(nextLevelId);
                break;
            default:
                SceneManager.LoadScene(sceneId);
                break;
        }
    }

    private int GetBuildSceneId()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

}
