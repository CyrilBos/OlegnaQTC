using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private static float DeathDelay = 1f;

    [SerializeField]
    private int maxHp = 100, maxResource = 50;

    private Animator animator;

    private int currentHp, currentResource;

    private List<PeriodicEffect> effects = new List<PeriodicEffect>();

    public static Action<int, Vector3> OnDamageTaken;
    public Action<float> OnDeath;
    public Action<int, int> OnHealthUpdated;

    public int CurrentResource { get => currentResource; }
    public Character Target { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        currentHp = maxHp;
        currentResource = maxResource;
    }

    private void OnMouseUp()
    {
        Player.Target = this;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        foreach (PeriodicEffect effect in effects)
        {
            Debug.Log(effect.Duration);
            effect.UpdateTimer(deltaTime, this);
        }

        effects.RemoveAll(effect => effect.Duration <= 0);
    }

    public void GetHit(int damage)
    {
        TakeDamage(damage);
        PlayHurtAnimation();
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp < 0)
        {
            currentHp = 0;
            animator.SetBool("Dead", true);
            OnDeath(animator.GetCurrentAnimatorStateInfo(0).length + Character.DeathDelay);
        }
        OnDamageTaken(amount, transform.position);
        OnHealthUpdated(currentHp, maxHp);
    }
    public void SpendResource(int amount)
    {
        if (currentResource > amount)
        {
            currentResource -= amount;
        }
    }

    public bool IsDead()
    {
        return currentHp <= 0;
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