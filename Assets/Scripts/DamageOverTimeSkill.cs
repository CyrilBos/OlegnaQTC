using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeSkill : CooldownSkill
{
    [SerializeField]
    private int damagePerProc;

    [SerializeField]
    private float procFrequency, duration;

    protected override void SkillEffect()
    {
        Debug.Log($"{user.Target} received DoT of {damagePerProc} every {procFrequency}s for {duration}s");
        user.Target.AddStatusEffect(new DamageOverTime(procFrequency, duration, damagePerProc));
    }
}
