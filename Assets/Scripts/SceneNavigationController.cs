using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*This script handles user navigation between scenes
 * It can be on any one game object in the scene
 */
public class SceneNavigationController : MonoBehaviour
{
    //At any time, if the user presses Escape key the main menu scene will load.
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
    //This works because the game has only 1 game scene and 1 main menu scene(0)
    public void StartGame()
    {
        LoadSceneByBuildIndex(1);
    }

}
