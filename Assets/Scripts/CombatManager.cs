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

    private void Awake()
    {
        mainCamera = Camera.main;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        player = GameObject.Find("Player").GetComponent<Player>();

        rightEnemies.Add(InstantiateEnemy(new Vector3(5, 0), Quaternion.identity));
        leftEnemies.Add(InstantiateEnemy(new Vector3(-5, 0), Quaternion.Euler(0, 180, 0)));
    }

    private void Start()
    {
        AssignNewTarget(rightEnemies, false);
    }

    private Character InstantiateEnemy(Vector2 position, Quaternion rotation)
    {
        GameObject enemyGO = Instantiate(enemyPrefab, position, rotation);

        Enemy enemy = enemyGO.GetComponent<Enemy>();
        enemy.Character.InitStats(100, 50);
        enemy.Character.OnDeath += EnemyDied;

        GameObject healthBar = Instantiate(healthBarPrefab);
        healthBar.transform.SetParent(canvas.transform, false);
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(position);
        screenPosition.y += 100;
        healthBar.transform.position = screenPosition;

        healthBar.GetComponent<HealthBar>().SetCharacter(enemy.Character);

        return enemy.Character;
    }

    private void EnemyDied(Character enemy, float _)
    {
        enemy.OnDeath -= EnemyDied;
        if (rightEnemies.Contains(enemy))
        {
            rightEnemies.Remove(enemy);
            AssignNewTarget(rightEnemies, false);
        } else if (leftEnemies.Contains(enemy))
        {
            leftEnemies.Remove(enemy);
            AssignNewTarget(leftEnemies, true);
        }
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
