using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab, healthBarPrefab;

    private Camera mainCamera;

    private Canvas canvas;

    private Player player;

    private List<Character> rightEnemies = new List<Character>(), leftEnemies = new List<Character>();
    private Queue<Character> incomingRightEnemies = new Queue<Character>(), incomingLeftEnemies = new Queue<Character>();

    private static Vector2 FightingLeftEnemyPosition = new Vector2(5, 0), FightingRightEnemyPosition = new Vector2(-5, 0);
    private static Vector2 IncomingLeftEnemyPosition = new Vector2(8, 0), IncomingRightEnemyPosition = new Vector2(-8, 0);

    private void Awake()
    {
        mainCamera = Camera.main;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        player = GameObject.Find("Player").GetComponent<Player>();

        rightEnemies.Add(InstantiateEnemy(FightingLeftEnemyPosition, Quaternion.identity, true));
        leftEnemies.Add(InstantiateEnemy(FightingRightEnemyPosition, Quaternion.Euler(0, 180, 0), true));

        incomingRightEnemies.Enqueue(InstantiateEnemy(IncomingLeftEnemyPosition, Quaternion.identity));
        incomingLeftEnemies.Enqueue(InstantiateEnemy(IncomingRightEnemyPosition, Quaternion.Euler(0, 180, 0)));
    }

    private void Start()
    {
        AssignNewTarget(rightEnemies, false);
    }

    private Character InstantiateEnemy(Vector2 position, Quaternion rotation, bool isFighting = false)
    {
        GameObject enemyGO = Instantiate(enemyPrefab, position, rotation);

        Enemy enemy = enemyGO.GetComponent<Enemy>();
        

        enemy.Character.InitStats(100, 50);

        if (isFighting)
        {
            enemy.StartFighting();
            InstantiateAndAssignHealthBar(position, enemy);

            enemy.Character.OnDeath += RemoveDeadEnemyAndPushNewEnemyAndAssignNewTarget;
        }


        return enemy.Character;
    }

    // On Enemy Death 
    private void RemoveDeadEnemyAndPushNewEnemyAndAssignNewTarget(Character enemy)
    {
        enemy.OnDeath -= RemoveDeadEnemyAndPushNewEnemyAndAssignNewTarget;
        if (rightEnemies.Contains(enemy))
        {
            rightEnemies.Remove(enemy);
            PushIncomingEnemy(incomingRightEnemies, rightEnemies, FightingLeftEnemyPosition);
            AssignNewTarget(rightEnemies, false);
        }
        else if (leftEnemies.Contains(enemy))
        {
            leftEnemies.Remove(enemy);
            PushIncomingEnemy(incomingLeftEnemies, leftEnemies, FightingRightEnemyPosition);
            AssignNewTarget(leftEnemies, true);
        }

        if (rightEnemies.Count < 1 && leftEnemies.Count < 1 && incomingLeftEnemies.Count < 1 && incomingRightEnemies.Count < 1)
        {
            Debug.Log("Win!"); // TODO
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
        healthBar.transform.SetParent(canvas.transform, false);
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(position);
        screenPosition.y += 100;
        healthBar.transform.position = screenPosition;

        healthBar.GetComponent<HealthBar>().SetCharacter(enemy.Character);
    }

    // Will assign a new target to the player, prioritizing the side given in parameter
    private void AssignNewTarget(List<Character> enemies, bool isLeft)
    {
        if (SetNewTargetIfEnemiesRemain(enemies, isLeft))
        {
            if (enemies == leftEnemies)
            {
                SetNewTargetIfEnemiesRemain(rightEnemies, false);
            }
            else
            {
                SetNewTargetIfEnemiesRemain(leftEnemies, true);
            }
        }
    }

    // Will return false if enemies remain
    private bool SetNewTargetIfEnemiesRemain(List<Character> enemies, bool isLeft)
    {
        if (enemies.Count > 0)
        {
            player.ChangeTarget(enemies[0], isLeft);
            return false;
        }

        return true;
    }
}
