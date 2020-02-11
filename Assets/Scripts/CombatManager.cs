using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField]
    GameObject enemyPrefab, // TODO: replace with lists of enemies using ScriptableObjects or multiple prefabs 
        healthBarPrefab;

    private GameManager _gameManager;

    private Camera _mainCamera;

    [SerializeField]
    private Canvas _canvas;

    private static Player s_player;
    
    public static Player Player
    {
        get { return s_player; }
    }

    [SerializeField]
    private readonly List<Character> _rightEnemies = new List<Character>();
    private readonly List<Character> _leftEnemies = new List<Character>();
    private readonly Queue<Character> _incomingRightEnemies = new Queue<Character>();
    private readonly Queue<Character> _incomingLeftEnemies = new Queue<Character>();

    private static readonly Vector2 FightingLeftEnemyPosition = new Vector2(5, 0);
    private static readonly Vector2 FightingRightEnemyPosition = new Vector2(-5, 0);
    private static readonly Vector2 IncomingLeftEnemyPosition = new Vector2(8, 0);
    private static readonly Vector2 IncomingRightEnemyPosition = new Vector2(-8, 0);

    private new void Awake() // TODO: new?
    {
        base.Awake();
        _mainCamera = Camera.main;
        s_player = GameObject.Find("Player").GetComponent<Player>();
        _canvas.gameObject.SetActive(true);

        _rightEnemies.Add(InstantiateEnemy(FightingLeftEnemyPosition, Quaternion.identity, true));
        _leftEnemies.Add(InstantiateEnemy(FightingRightEnemyPosition, Quaternion.Euler(0, 180, 0), true));

        _incomingRightEnemies.Enqueue(InstantiateEnemy(IncomingLeftEnemyPosition, Quaternion.identity));
        _incomingLeftEnemies.Enqueue(InstantiateEnemy(IncomingRightEnemyPosition, Quaternion.Euler(0, 180, 0)));
    }

    private void Start()
    {
        AssignNewTarget(_rightEnemies, false);
    }

    private Character InstantiateEnemy(Vector2 position, Quaternion rotation, bool isFighting = false)
    {
        GameObject enemyGo = Instantiate(enemyPrefab, position, rotation);

        Enemy enemy = enemyGo.GetComponent<Enemy>();
        

        enemy.Character.InitStats(100, 50);

        if (isFighting)
        {
            enemy.StartFighting();
            InstantiateAndAssignHealthBar(position, enemy);

            enemy.Character.OnDeath += RemoveDeadEnemyAndPushNewEnemyAndAssignNewTarget;
        }


        return enemy.Character;
    }

    // Called on enemy death 
    private void RemoveDeadEnemyAndPushNewEnemyAndAssignNewTarget(Character enemy)
    {
        enemy.OnDeath -= RemoveDeadEnemyAndPushNewEnemyAndAssignNewTarget;
        if (_rightEnemies.Contains(enemy))
        {
            _rightEnemies.Remove(enemy);
            PushIncomingEnemy(_incomingRightEnemies, _rightEnemies, FightingLeftEnemyPosition);
            AssignNewTarget(_rightEnemies, false);
        }
        else if (_leftEnemies.Contains(enemy))
        {
            _leftEnemies.Remove(enemy);
            PushIncomingEnemy(_incomingLeftEnemies, _leftEnemies, FightingRightEnemyPosition);
            AssignNewTarget(_leftEnemies, true);
        }

        if (_rightEnemies.Count < 1 && _leftEnemies.Count < 1 && _incomingLeftEnemies.Count < 1 && _incomingRightEnemies.Count < 1)
        {
            Debug.Log("Win!");
            _gameManager.LoadLevel("CombatOver");
        }
    }

    private void PushIncomingEnemy(Queue<Character> incomingEnemies, List<Character> enemies, Vector2 position)
    {
        if (incomingEnemies.Count > 0)
        {
            Character newEnemyCharacter = incomingEnemies.Dequeue();
            Enemy newEnemy = newEnemyCharacter.GetComponent<Enemy>(); 

            newEnemy.GetComponent<Enemy>().GoInFight(position);
            InstantiateAndAssignHealthBar(position, newEnemy);
            newEnemyCharacter.OnDeath += RemoveDeadEnemyAndPushNewEnemyAndAssignNewTarget;

            enemies.Add(newEnemyCharacter);
        }
    }

    private void InstantiateAndAssignHealthBar(Vector2 position, Enemy enemy)
    {
        GameObject healthBar = Instantiate(healthBarPrefab);
        healthBar.transform.SetParent(_canvas.transform, false);
        Vector2 screenPosition = _mainCamera.WorldToScreenPoint(position);
        screenPosition.y += 100;
        healthBar.transform.position = screenPosition;

        healthBar.GetComponent<HealthBar>().SetCharacter(enemy.Character);
    }

    // Will assign a new target to the player, prioritizing the side given in parameter
    private void AssignNewTarget(List<Character> enemies, bool isLeft)
    {
        if (SetNewTargetIfEnemiesRemain(enemies, isLeft))
        {
            if (enemies == _leftEnemies)
            {
                SetNewTargetIfEnemiesRemain(_rightEnemies, false);
            }
            else
            {
                SetNewTargetIfEnemiesRemain(_leftEnemies, true);
            }
        }
    }

    // Will return false if enemies remain
    private bool SetNewTargetIfEnemiesRemain(List<Character> enemies, bool isLeft)
    {
        if (enemies.Count > 0)
        {
            s_player.ChangeTarget(enemies[0], isLeft);
            return false;
        }

        return true;
    }

    public bool isEnemyOnLeftSide(Character character)
    {
        return _leftEnemies.Contains(character);
    }
}
