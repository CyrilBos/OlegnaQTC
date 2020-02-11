using System;
using System.Collections;
using System.Collections.Generic;
using Skills;
using UnityEngine;

public enum EnemyState
{
    Fighting,
    Waiting,
    Moving
}

public class Enemy : MonoBehaviour
{
    public Character Character { get; private set; }

    private EnemyState _state = EnemyState.Waiting;

    private CooldownSkill[] _attacks;

    private Vector2 _targetPosition;

    private static float MovingSpeed = 0.1f;
    private static float MoveToFightPositionDelta = 0.001f;

    private void Awake()
    {
        Character = GetComponent<Character>();
        _attacks = GetComponents<CooldownSkill>();
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

        if (_state == EnemyState.Moving)
        {
            if (Vector2.Distance(transform.position, _targetPosition) < Enemy.MoveToFightPositionDelta)
            {
                StartFighting();
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, _targetPosition, Enemy.MovingSpeed);
            }
        } else if (_state == EnemyState.Fighting && !Character.IsInGlobalCooldown()) { 
            CooldownSkill skillToUse = ChooseSkill();
            if (skillToUse != null)
            {
                skillToUse.Use();
            }
        }
    }

    public void StartFighting()
    {
        _state = EnemyState.Fighting;
    }

    public void GoInFight(Vector2 position)
    {
         _targetPosition = position;
        _state = EnemyState.Moving;
    }

    private CooldownSkill ChooseSkill()
    {
        foreach (CooldownSkill attack in _attacks)
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
