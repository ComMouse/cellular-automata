using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance = null;
    public static GameController Instance => instance;

    public GameState state = GameState.None;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.Prepare);
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.Prepare:

                // if everyone is prepared

                // start game

                break;
            case GameState.Ongoing:
                break;
            case GameState.Result:

                // if restart is pressed

                // restart game

                break;
        }
    }

    public void ChangeState(GameState nextState)
    {
        if (state == nextState)
            return;

        switch (state)
        {
            case GameState.Prepare:
                break;
            case GameState.Ongoing:
                break;
            case GameState.Result:
                break;
        }

        state = nextState;

        switch (nextState)
        {
            case GameState.Prepare:
                break;
            case GameState.Ongoing:
                break;
            case GameState.Result:
                break;
        }
    }

    public void StartGame()
    {
        // TODO
    }
}

public enum GameState
{
    None = 0,
    Prepare = 1,
    Ongoing = 2,
    Result = 3,
}