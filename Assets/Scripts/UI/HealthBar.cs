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
        // for player that is assigned in editor
        if (character != null)
        {
            healthText.text = $"{character.CurrentHealth}/{character.MaxHealth}";
            character.OnHealthUpdated += UpdateHealth;
        }
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
        UpdateHealth(character.CurrentHealth, character.MaxHealth);
        character.OnHealthUpdated += UpdateHealth;
        character.OnDeath += DestroyHealthBar;
    }

    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    private void DestroyHealthBar(Character _)
    {
        Destroy(gameObject);
    }
}
