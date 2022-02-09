using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScript : MonoBehaviour
{

    [Header("Public Scripts :")]
    public ShowItemMenuScript ShowItemScript;
    public InventoryMenuScript InventoryScript;
    public GameOverMenuScript GameOverScript;
    public ClearLevelMenuScript ClearLevelScript;
    public ConnectionMenuScript ConnectionScript;

    [Header("GameObject names :")]
    public string WindowToShowPrize;
    public string WindowToShowInventory;
    public string WindowToShowGameOver;
    public string WindowToShowClearLevel;

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
