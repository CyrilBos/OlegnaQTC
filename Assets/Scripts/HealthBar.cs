using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField]
    private Character character;
    Text healthText;
    private void Awake()
    {
        healthText = GetComponentInChildren<Text>();
        character.OnHealthUpdated += UpdateHealth;
        character.OnDeath += DestroyHealthBar;
    }

    private void OnDisable()
    {
        character.OnHealthUpdated -= UpdateHealth;
        character.OnDeath -= DestroyHealthBar;
    }

    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    private void DestroyHealthBar(float deathAnimationLength)
    {
        Destroy(gameObject, deathAnimationLength);
    }
}
