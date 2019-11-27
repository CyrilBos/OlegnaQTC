using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CooldownSkill : MonoBehaviour
{
    [SerializeField]
    private float cooldown = 1;

    [SerializeField]
    private int resourceCost;

    [SerializeField]
    protected Character user;

    private float currentCooldown = 0f;
    public Action<float> OnCooldownUpdate;

    // Update is called once per frame
    void Update()
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
        return currentCooldown <= 0 && user.CurrentResource > resourceCost && !user.IsDead() && !user.Target.IsDead() && !user.IsInGlobalCooldown();
    }

    public void UseSkill()
    {
        if (IsUsable())
        {
            SkillEffect();
            user.SpendResource(resourceCost);
            user.TriggerGlobalCooldown();
            currentCooldown = cooldown;
        }
    }

    protected abstract void SkillEffect();
}
