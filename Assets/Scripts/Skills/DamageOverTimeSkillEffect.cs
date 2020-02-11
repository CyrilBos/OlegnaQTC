using UnityEngine;

[CreateAssetMenu(menuName = "Skills/DamageOverTimeSkillEffect")]
public class DamageOverTimeSkillEffect : SkillEffect
{
    [SerializeField]
    private int damagePerProc;

    [SerializeField]
    private float tickFrequency, duration;

    public override void ApplyEffect(Character target)
    {
        target.AddStatusEffect(new DamageOverTime(tickFrequency, duration, damagePerProc));
    }
}
