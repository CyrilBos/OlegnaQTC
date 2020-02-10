using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState
{
    COMBAT,
    COMBAT_OVER
}

public class GameManager : Singleton<GameManager>
{

    private GameState currentState;

    private string currentLevelName = string.Empty;

    private List<AsyncOperation> _loadOperations = new List<AsyncOperation>();

    private CombatManager combatManager;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadLevel("Combat");
        SwitchState(GameState.COMBAT);
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError($"Unable to load {levelName}");
            return;
        }
        ao.completed += RemoveLoadOperation;
        _loadOperations.Add(ao);

        currentLevelName = levelName;
    }

    private void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError($"Unable to unload {levelName}");
        }
    }

    private void RemoveLoadOperation(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
        }
    }

    private void SwitchState(GameState newState)
    {
        this.currentState = newState;
    }
}
