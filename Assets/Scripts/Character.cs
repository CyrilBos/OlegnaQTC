using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private static float DeathDelay = 1f;

    private float globalCooldown = 1f;
    private static float GlobalCooldownDuration = 1f;

    [SerializeField]
    private int maxHealth = 100, maxResource = 50;

    private Animator animator;

    private List<PeriodicEffect> effects = new List<PeriodicEffect>();

    public Character Target { get; set; }

    public bool IsInGlobalCooldown()
    {
        return globalCooldown > 0;
    }

    public int CurrentResource { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    public static Action<int, Vector3> OnDamageTaken;
    public Action<Character, float> OnDeath;
    public Action<int, int> OnHealthUpdated;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        CurrentHealth = maxHealth;
        CurrentResource = maxResource;
    }

    private void OnMouseUp()
    {
        Player.Target = this;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (IsInGlobalCooldown())
        {
            globalCooldown -= deltaTime;
        }

        foreach (PeriodicEffect effect in effects)
        {
            Debug.Log(effect.Duration);
            effect.UpdateTimer(deltaTime, this);
        }

        effects.RemoveAll(effect => effect.Duration <= 0);
    }

    public void InitStats(int health, int resource)
    {
        CurrentHealth = maxHealth = health;
        CurrentResource = maxResource = resource;
    }

    public void GetHit(int damage)
    {
        TakeDamage(damage);
        PlayHurtAnimation();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
            animator.SetBool("Dead", true);
            OnDeath(this, animator.GetCurrentAnimatorStateInfo(0).length + Character.DeathDelay);
        }
        OnDamageTaken(amount, transform.position);
        OnHealthUpdated(CurrentHealth, maxHealth);
    }

    public void TriggerGlobalCooldown()
    {
        globalCooldown = GlobalCooldownDuration;
    }

    public void SpendResource(int amount)
    {
        if (CurrentResource > amount)
        {
            CurrentResource -= amount;
        }
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    public void AddStatusEffect(PeriodicEffect statusEffect)
    {
        effects.Add(statusEffect);
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    private void PlayHurtAnimation()
    {
        animator.SetTrigger("Hurt");
    }
}