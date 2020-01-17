using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    FIGHTING,
    WAITING,
    MOVING
}

public class Enemy : MonoBehaviour
{
    public Character Character { get; set; }

    private EnemyState state = EnemyState.WAITING;

    private CooldownSkill[] attacks;

    private Vector2 targetPosition;

    private static float MovingSpeed = 0.1f;
    private static float MoveToFightPositionDelta = 0.001f;

    private void Awake()
    {
        Character = GetComponent<Character>();
        attacks = GetComponents<CooldownSkill>();
        Character.Target = GameObject.Find("Player").GetComponent<Character>();
        Character.OnDeath += RemoveBody;
    }

    private void OnDisable()
    {
        Character.OnDeath -= RemoveBody;
    }

    private void Update()
    {
        if (Player.IsDead() || Character.IsDead())
            return;

        if (state == EnemyState.MOVING)
        {
            if (Vector2.Distance(transform.position, targetPosition) < Enemy.MoveToFightPositionDelta)
            {
                StartFighting();
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, Enemy.MovingSpeed);
            }
        } else if (state == EnemyState.FIGHTING && !Character.IsInGlobalCooldown()) { 
            CooldownSkill skillToUse = ChooseSkill();
            if (skillToUse != null)
            {
                skillToUse.UseSkill();
            }
        }
    }

    public void StartFighting()
    {
        state = EnemyState.FIGHTING;
    }

    public void GoInFight(Vector2 position)
    {
         targetPosition = position;
        state = EnemyState.MOVING;
    }

    private CooldownSkill ChooseSkill()
    {
        foreach (CooldownSkill attack in attacks)
        {
            if (attack.IsUsable())
            {
                return attack;
            }
        }
        return null;
    }

    private void RemoveBody(Character _)
    {
        Destroy(gameObject);
    }
}
