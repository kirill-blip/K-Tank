using UnityEngine;
public class DataManager : MonoBehaviour
{
    public bool deleteData = false;
    private void Awake()
    {
        if (deleteData)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void SavePlayerData(PlayerController playerController)
    {
        if (playerController.canMoveOnWater)
            PlayerPrefs.SetInt("HaveBoat", 1);
        if (playerController.turboShooting)
            PlayerPrefs.SetInt("TurboShooting", 1);
        if (playerController.canDestroyBush)
            PlayerPrefs.SetInt("CanDestroyBush", 1);
        if (playerController.canDestroyIron)
            PlayerPrefs.SetInt("CanDestroyIron", 1);
    }
    public void LoadPlayerData(PlayerController playerController)
    {
        if (PlayerPrefs.GetInt("HaveBoat") == 1)
        {
            playerController.canMoveOnWater = true;
            playerController.boatGO.SetActive(true);
        }
        if (PlayerPrefs.GetInt("TurboShooting") == 1)
        {
            playerController.turboShooting = true;
        }
        if (PlayerPrefs.GetInt("CanDestroyBush") == 1)
        {
            playerController.canDestroyBush = true;
        }
        if (PlayerPrefs.GetInt("CanDestroyIron") == 1)
        {
            playerController.canDestroyIron = true;
        }
    }
}