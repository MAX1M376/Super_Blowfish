using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScript : MonoBehaviour
{
    private void Start()
    {
        Gameplay();
    }

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Gameplay");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void Paused()
    {
        GameStateManager.Instance.SetState(GameState.Paused);
        PlayerBehaviour.controlEnabled = false;
    }

    public void Gameplay()
    {
        GameStateManager.Instance.SetState(GameState.GamePlay);
        PlayerBehaviour.controlEnabled = true;
    }
}
