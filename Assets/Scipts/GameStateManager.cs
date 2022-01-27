using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager
{
    private static GameStateManager instance;

    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameStateManager();
            return instance;
        }
    }

    public delegate void GameStateChangeHandler(GameState state);
    public event GameStateChangeHandler OnGameStateChange;

    public GameState ActualState { get; private set; }

    public void SetState(GameState state)
    {
        if (state == ActualState)
            return;

        ActualState = state;
        OnGameStateChange?.Invoke(state);
    }
}
