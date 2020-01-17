using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/DamageSkillEffect")]
public class DamageSkillEffect : SkillEffect
{
    [SerializeField]
    private int damage;

    public override void ApplyEffect(Character target)
    {
        target.TakeDamage(damage);
    }
}
