using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigationController : MonoBehaviour
{
    [SerializeField] GameObject PlayerDisplay;
    DisplayUIControl displayUI;


    private void Awake()
    {
        if(PlayerDisplay!=null)
        {
            displayUI = PlayerDisplay.GetComponent<DisplayUIControl>();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LoadScene("MainMenu");

        }
    }

    /* scene controllers *************************************/
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByBuildIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Application.");
    }

    public void StartGame()
    {
        LoadSceneByBuildIndex(1);
    }

}
