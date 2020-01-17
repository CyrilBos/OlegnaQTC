public class DamageOverTime : PeriodicEffect
{
    private int damagePerProc;

    public DamageOverTime(float tickFrequency, float duration, int damagePerProc) : base(tickFrequency, duration)
    {
        this.damagePerProc = damagePerProc;
    }

    public override void ApplyEffect(Character character)
    {
        character.TakeDamage(damagePerProc);
    }
}