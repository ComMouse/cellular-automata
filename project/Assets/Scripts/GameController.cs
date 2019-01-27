using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController instance = null;
    public static GameController Instance => instance;

    public GameState state = GameState.None;

    public PrepareController prepareCtrl;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.None:
                if (prepareCtrl != null)
                {
                    ChangeState(GameState.Prepare);
                }

                break;
            case GameState.Prepare:
                if (prepareCtrl == null)
                    return;
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
                if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0))
                {
                    StartCoroutine(LoadTitleAsync());
                }
                else
                {
                    prepareCtrl.Init();
                }

                break;
            case GameState.Load:
                StartCoroutine(LoadGameAsync());
                break;
            case GameState.Ongoing:
                break;
            case GameState.Result:
                break;
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.Load);
    }

    private IEnumerator LoadTitleAsync()
    {
        yield return new WaitForSeconds(1f);

        LoadScene(SceneType.Title);

        yield return null;

        prepareCtrl.Init();
    }

    private IEnumerator LoadGameAsync()
    {
        yield return new WaitForSeconds(2f);

        LoadScene(SceneType.Game);

        ChangeState(GameState.Ongoing);

        SoundManager.Instance.Play("Music_MemoCute", 1f);
    }

    private void LoadScene(SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.Title:
                SceneManager.LoadScene(0, LoadSceneMode.Single);
                break;
            case SceneType.Game:
                SceneManager.LoadScene(1, LoadSceneMode.Single);
                break;
        }
    }
}

public enum GameState
{
    None = 0,
    Prepare = 1,
    Load = 2,
    Ongoing = 3,
    Result = 4,
}

public enum SceneType
{
    Title = 0,
    Game = 1
}