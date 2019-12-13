using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;


public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            DontDestroyOnLoad(FindObjectOfType<Settings>());
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            DontDestroyOnLoad(FindObjectOfType<Core>());
        }
        else if (SceneManager.GetActiveScene().name == "Stats")
        {
            foreach (Core core in FindObjectsOfType<Core>())
            {
                Destroy(core.gameObject);
            }

            foreach (Settings settings in FindObjectsOfType<Settings>())
            {
                Destroy(settings.gameObject);
            }
        }

        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}