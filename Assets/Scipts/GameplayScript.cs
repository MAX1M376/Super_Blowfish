using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScript : MonoBehaviour
{
    private void Start()
    {
        Gameplay();
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
