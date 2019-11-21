using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSkill : CooldownSkill
{
    [SerializeField]
    private int damage = 10;



    protected override void SkillEffect()
    {
        Debug.Log($"{user} uses skill {gameObject}");
        user.Target.GetHit(damage);
        user.PlayAttackAnimation();
    }
}
