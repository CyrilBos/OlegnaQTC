using UnityEngine;

public abstract class SkillEffect : ScriptableObject
{
    public abstract void ApplyEffect(Character target);
}
