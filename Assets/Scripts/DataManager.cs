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
}

