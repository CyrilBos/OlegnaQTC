public class DamageOverTime : PeriodicEffect
{
    private int damagePerProc;

    public DamageOverTime(float procFrequency, float duration, int damagePerProc) : base(procFrequency, duration)
    {
        this.damagePerProc = damagePerProc;
    }

    public override void ApplyEffect(Character character)
    {
        character.TakeDamage(damagePerProc);
    }
}