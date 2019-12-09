using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;


public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        DontDestroyOnLoad(FindObjectOfType<Settings>());
        SceneManager.LoadScene(sceneName);

        if (FindObjectsOfType<Settings>().Length > 1)
        {
            Destroy(FindObjectOfType<Settings>());
        }
    }
}