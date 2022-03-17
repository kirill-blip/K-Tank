using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public GameObject panel;
    public Text countOfEnemiesText;
    public Text healthText;
    private AudioManager audioManager;
    private PlayerController playerController;
    private EnemySpawnManager enemySpawnManager;
    private LevelManager levelManager;
    private void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>().GetComponent<AudioManager>();

        try
        {
            levelManager = GameObject.FindObjectOfType<LevelManager>();
        }
        catch (Exception e)
        {
            Debug.Log($"LevelManager doesn't exist. {0}");
            throw;
        }

        try
        {
            enemySpawnManager = GameObject.FindObjectOfType<EnemySpawnManager>().GetComponent<EnemySpawnManager>();
            enemySpawnManager.countOfEnemiesChanged += UpdateCountOfEnemies;
        }
        catch (Exception e)
        {
            Debug.Log($"EnemySpawnManager doesn't exist. {e}");
        }

        try
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.playerDamaged += PlayerController_playerDamaged;
            healthText.text = "Health: " + playerController.GetHealth();
        }
        catch (Exception e)
        {
            Debug.Log($"PlayerController doesn't exitst. {e}");
        }
    }


    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && panel != null)
            ActivePanel();
    }

    public void ActivePanel()
    {
        panel.SetActive(!panel.activeInHierarchy);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
    private void PlayerController_playerDamaged(object sender, GameObject e)
    {
        healthText.text = "Health: " + playerController.GetHealth();
    }
    private void UpdateCountOfEnemies(object sender, int enemiesCount)
    {
        countOfEnemiesText.text = "Count of enemies: " + enemiesCount;
    }
    private IEnumerator WaitForExitGame()
    {
        yield return new WaitForSeconds(0.25f);
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    // Methods for UI buttons
    public void LoadScene(int id)
    {
        StartCoroutine(levelManager.LoadSceneByButton(id, 0.125f));
    }
    public void ExitGame()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitForExitGame());
    }
    public void PlayClickingSound()
    {
        audioManager.PlaySound(SoundName.Clicking);
    }
}