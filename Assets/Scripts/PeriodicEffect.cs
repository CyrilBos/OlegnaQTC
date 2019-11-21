public abstract class PeriodicEffect
{
    private float procFrequency;
    private float currentTimer = 0;

    public float Duration { get; set; }

    public PeriodicEffect(float procFrequency, float duration)
    {
        this.procFrequency = procFrequency;
        this.Duration = duration;
    }

    public void UpdateTimer(float timePassed, Character character)
    {
        currentTimer -= timePassed;
        Duration = Duration - timePassed;
        if (currentTimer <= 0 && Duration > 0)
        {
            ApplyEffect(character);
            currentTimer = procFrequency;
        }
    }
    public abstract void ApplyEffect(Character character);
}