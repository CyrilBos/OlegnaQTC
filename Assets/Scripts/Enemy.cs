using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AttackSkill[] attacks;

    public Character Character { get; set; }

    private void Awake()
    {
        Character = GetComponent<Character>();
        Character.Target = GameObject.Find("Player").GetComponent<Character>();
        Character.OnDeath += Die;
    }

    private void OnDisable()
    {
        Character.OnDeath -= Die;
    }

    private void Update()
    {
        if (Player.IsDead() || Character.IsDead())
            return;

        if (!Character.IsInGlobalCooldown()) { 
            AttackSkill skillToUse = ChooseSkill();
            if (skillToUse != null)
            {
                skillToUse.UseSkill();
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

    private void Die(Character _, float deathAnimationLength)
    {
        Destroy(gameObject, deathAnimationLength);
    }
}
