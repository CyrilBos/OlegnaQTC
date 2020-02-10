using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownSkill : MonoBehaviour
{
    [SerializeField]
    private new String name;

    [SerializeField]
    private float cooldown = 1;

    [SerializeField]
    private int resourceCost;

    [SerializeField]
    private SkillEffect[] effects;

    [SerializeField]
    private Character user;

    [SerializeField]
    private String animationStateName; 

    private float currentCooldown = 0f;

    public string Name { get => name; }
    public float Cooldown { get => cooldown; }
    public float CurrentCooldown { get => currentCooldown; }

    public Action<float> OnCooldownUpdate;

    private void Awake()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            if (OnCooldownUpdate != null)
                OnCooldownUpdate(currentCooldown);
        }
    }

    public bool IsUsable()
    {
        return currentCooldown <= 0 && user.CurrentResource >= resourceCost && !user.IsDead() && !user.Target.IsDead() && !user.IsInGlobalCooldown() && !user.IsDodging();
    }

    public void UseSkill()
    {
        if (IsUsable())
        {
            if (user.Target.DodgesSkill(this))
            {
                return;
            }
            
            foreach (SkillEffect effect in effects)
            {
                effect.ApplyEffect(user.Target);
            }
            user.SpendResource(resourceCost);
            user.TriggerGlobalCooldown();
            user.ChangeAnimationState(animationStateName);
            currentCooldown = cooldown;

            // triggers hurt animation
            user.Target.GetHit();
        }
    }
}
