using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ChangeScene(int buildIndex)
    {
        try
        {
            SceneManager.LoadScene(buildIndex);
        }
        catch (System.Exception)
        {
            Debug.LogWarning("Scene don't exist");
        }
    }
}
