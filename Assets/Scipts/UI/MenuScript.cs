using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private GameplayScript gameplay;

    private void Awake()
    {
        var gameplayGO = GameObject.FindGameObjectWithTag("Gameplay");
        if (gameplayGO != null)
        {
            gameplay = gameplayGO.GetComponent<GameplayScript>();
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex > 0)
            {
                Debug.LogError("Gameplay components not load");
            }
        }
    }
    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            gameplay.Paused();
        }
    }

    private void OnDisable()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            gameplay.Gameplay();
        }
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

    public void NextScene()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
