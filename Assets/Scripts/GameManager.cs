using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState
{
    Combat,
    CombatOver
}

public class GameManager : Singleton<GameManager>
{

    private GameState _currentState;

    private string _currentLevelName = string.Empty;

    private readonly List<AsyncOperation> _loadOperations = new List<AsyncOperation>();

    private CombatManager _combatManager;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadLevel("Combat");
        SwitchState(GameState.Combat);
    }

    private void OnDisable()
    {
        foreach (AsyncOperation ao in _loadOperations)
        {
            ao.completed -= RemoveLoadOperation;
        }
        _loadOperations.Clear(); // TODO
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

        _currentLevelName = levelName;
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
        this._currentState = newState;
    }
}
