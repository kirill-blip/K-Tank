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
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && panel != null)
            ActivePanel();
    }

    public void LoadScene(int id)
    {
        StartCoroutine(WaitForLoadScene(id));
    }
    private IEnumerator WaitForLoadScene(int id)
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.125f);
        SceneManager.LoadScene(id);
    }
    public void ExitGame()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitForExitGame());
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

    public void ActivePanel()
    {
        panel.SetActive(!panel.activeInHierarchy);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
    public void PlayClickingSound()
    {
        audioManager.PlaySound(SoundName.Clicking);
    }
}