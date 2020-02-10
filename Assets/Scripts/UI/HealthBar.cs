using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField]
    private Character character;

    private Text healthText;
    private RectTransform barRect;

    private void Awake()
    {
        healthText = GetComponentInChildren<Text>();
        barRect = transform.Find("barImage").GetComponent<RectTransform>();
        // for player that is assigned in editor
        if (character != null)
        {
            healthText.text = $"{character.CurrentHealth}/{character.MaxHealth}";
            character.OnHealthUpdated += UpdateHealth;
        }
    }

    private void OnDisable()
    {
        character.OnHealthUpdated -= UpdateHealth;
        character.OnDeath -= DestroyHealthBar;
    }

    public void SetCharacter(Character barCharacter)
    {
        character = barCharacter;
        UpdateHealth(character.CurrentHealth, character.MaxHealth);
        character.OnHealthUpdated += UpdateHealth;
        character.OnDeath += DestroyHealthBar;
    }

    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
        barRect.localScale = new Vector3((float) currentHealth / maxHealth, 1, 1);
    }

    private void DestroyHealthBar(Character _)
    {
        Destroy(gameObject);
    }
}
