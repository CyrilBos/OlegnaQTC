using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSkill : MonoBehaviour
{
    [SerializeField]
    private float cooldown = 1;

    [SerializeField]
    private int damage = 10, resourceCost = 0;

    [SerializeField]
    private Character user;

    private float currentCooldown = 0f;
    public Action<float> OnCooldownUpdate;

    private Button button;
    private Text cooldownText;

    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            OnCooldownUpdate(currentCooldown);
        }
    }

    public bool IsUsable()
    {
        return currentCooldown <= 0 && user.CurrentResource > resourceCost && !user.IsDead() && !user.Target.IsDead();
    }

    public void UseSkill()
    {
        if (IsUsable())
        {
            user.Target.TakeDamage(damage);
            user.SpendResource(resourceCost);
            currentCooldown = cooldown;
            user.PlayAttackAnimation();
        }
    }
}
