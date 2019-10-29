using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AttackSkill[] attacks;

    private Character character;

    private float globalCooldown = 1f;
    private static float GlobalCooldownDuration = 1f;

    private void Awake()
    {
        character = GetComponent<Character>();
        character.Target = GameObject.Find("Player").GetComponent<Character>();
        character.OnDeath += Die;
    }

    private void OnDisable()
    {
        Player.EnemyDied(character);
        character.OnDeath -= Die;
    }

    private void Update()
    {
        if (Player.IsDead() || character.IsDead())
            return;

        if (globalCooldown > 0)
        {
            globalCooldown -= Time.deltaTime;
        }
        else
        {
            AttackSkill skillToUse = ChooseSkill();
            if (skillToUse != null)
            {
                Debug.Log($"{gameObject} uses {skillToUse}");
                // skillToUse.UseSkill();
                globalCooldown = GlobalCooldownDuration;
            }
        }
    }

    private AttackSkill ChooseSkill()
    {
        foreach (AttackSkill attack in attacks)
        {
            if (attack.IsUsable())
            {
                return attack;
            }
        }
        return null;
    }

    private void Die(float deathAnimationLength)
    {
        Destroy(gameObject, deathAnimationLength);
    }
}
