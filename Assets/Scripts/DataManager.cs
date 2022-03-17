using UnityEngine;
public class DataManager : MonoBehaviour
{
    public bool deleteData = false;

    private GameManager gameManager;
    private void Awake()
    {
        if (deleteData)
        {
            PlayerPrefs.DeleteAll();
        }
        try
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
        }
        catch
        {
            Debug.Log("GameManager doesn't exist");
        }
    }

    public void SavePlayerData()
    {
        if (gameManager.GetPlayerController().canMoveOnWater)
            PlayerPrefs.SetInt("HaveBoat", 1);
        if (gameManager.GetPlayerController().turboShooting)
            PlayerPrefs.SetInt("TurboShooting", 1);
        if (gameManager.GetPlayerController().canDestroyBush)
            PlayerPrefs.SetInt("CanDestroyBush", 1);
        if (gameManager.GetPlayerController().canDestroyIron)
            PlayerPrefs.SetInt("CanDestroyIron", 1);
    }
    public void LoadPlayerData(PlayerController playerController)
    {
        if (PlayerPrefs.GetInt("HaveBoat") == 1)
        {
            playerController.ActivateBoat();
        }
        if (PlayerPrefs.GetInt("TurboShooting") == 1)
        {
            playerController.turboShooting = true;
        }
        if (PlayerPrefs.GetInt("CanDestroyBush") == 1)
        {
            playerController.CanDestroyBush();
        }
        if (PlayerPrefs.GetInt("CanDestroyIron") == 1)
        {
            playerController.canDestroyIron = true;
        }
    }
}