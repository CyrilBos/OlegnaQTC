using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private float globalCooldown = 0f;
    public static float GlobalCooldownDuration = 1f;

    [SerializeField]
    private int maxHealth = 100, maxResource = 50;

    private Animator animator;

    private List<PeriodicEffect> effects = new List<PeriodicEffect>();

    // To store added effects during a frame to avoid modifying the effects list being in the Update() loop
    private Queue<PeriodicEffect> incomingEffects = new Queue<PeriodicEffect>();

    public Character Target { get; set; }

    public int CurrentResource { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int MaxResource { get => maxResource; set => maxResource = value; }


    public static Action<int, Vector3> OnDamageTaken;
    public Action<Character> OnDeath;
    public Action<int, int> OnHealthUpdated, OnResourceUpdated;
    public Action<float> OnGlobalCooldown;

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
        if (!IsDead())
        {
            float deltaTime = Time.deltaTime;

            if (IsInGlobalCooldown())
            {
                globalCooldown -= deltaTime;
                if (OnGlobalCooldown != null)
                    OnGlobalCooldown(globalCooldown);
            }

            while (incomingEffects.Count > 0)
            {
                effects.Add(incomingEffects.Dequeue());
            }

            foreach (PeriodicEffect effect in effects)
            {
                effect.UpdateTimerAndTick(deltaTime, this);
                // in case last effect processed killed the character, get out of the loop to avoid unnecessary work and errors
                if (IsDead())
                    break;
            }

            effects.RemoveAll(effect => effect.Duration <= 0);
        }
    }

    public void InitStats(int health, int resource)
    {
        CurrentHealth = maxHealth = health;
        CurrentResource = maxResource = resource;
    }

    public bool IsInGlobalCooldown()
    {
        return globalCooldown > 0;
    }

    public void GetHit()
    {
        PlayHurtAnimation();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        if (IsDead())
        {
            CurrentHealth = 0;
            effects.Clear();
            animator.SetBool("Dead", true);
        }
        OnDamageTaken(amount, transform.position);
        OnHealthUpdated(CurrentHealth, maxHealth);
    }

    public void TriggerGlobalCooldown()
    {
        globalCooldown = GlobalCooldownDuration;
        if (OnGlobalCooldown != null)
            OnGlobalCooldown(GlobalCooldownDuration);
    }

    public void SpendResource(int amount)
    {
        if (CurrentResource >= amount)
        {
            CurrentResource -= amount;
            if (OnResourceUpdated != null)
            {
                OnResourceUpdated(CurrentResource, maxResource);
            }
        }
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    public void AddStatusEffect(PeriodicEffect statusEffect)
    {
        incomingEffects.Enqueue(statusEffect);
    }

    public void ChangeAnimationState(String stateName)
    {
        animator.Play(stateName);
    }

    public void Die()
    {
        OnDeath(this);
    }


    private void PlayHurtAnimation()
    {
        animator.SetTrigger("Hurt");
    }
}