public abstract class PeriodicEffect
{
    private float tickFrequency;
    private float currentTimer = 0;

    public float Duration { get; set; }

    public PeriodicEffect(float tickFrequency, float duration)
    {
        this.tickFrequency = tickFrequency;
        this.Duration = duration;
    }

    public void UpdateTimerAndTick(float timePassed, Character character)
    {
        currentTimer -= timePassed;
        Duration = Duration - timePassed;
        if (currentTimer <= 0 && Duration > 0)
        {
            ApplyEffect(character);
            currentTimer = tickFrequency;
        }
    }
    public abstract void ApplyEffect(Character character);
}