using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && panel != null)
            ActivePanel();
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    public void ActivePanel()
    {
        panel.SetActive(!panel.activeInHierarchy);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
}
