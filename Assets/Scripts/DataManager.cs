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
        if (gameManager.GetPlayerController().CanMoveOnWater())
            PlayerPrefs.SetInt("HaveBoat", 1);
        if (gameManager.GetPlayerController().HaveTurboShooting())
            PlayerPrefs.SetInt("TurboShooting", 1);
        if (gameManager.GetPlayerController().CanDestroyBush())
            PlayerPrefs.SetInt("CanDestroyBush", 1);
        if (gameManager.GetPlayerController().CanDestroyIron())
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
            playerController.ShootingBonus();
        }
        if (PlayerPrefs.GetInt("CanDestroyBush") == 1)
        {
            playerController.SetActiveDestoyBush();
        }
        if (PlayerPrefs.GetInt("CanDestroyIron") == 1)
        {
            playerController.ShootingBonus();
        }
    }
}