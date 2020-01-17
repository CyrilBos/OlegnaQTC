using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: refactor with HealthBar that has the same behaviour
public class ResourceBar : MonoBehaviour
{
    [SerializeField]
    private Character character;
    Text resourceText;

    private void Awake()
    {
        resourceText = GetComponentInChildren<Text>();
        resourceText.text = $"{character.CurrentResource}/{character.MaxResource}";
        // for player that is assigned in editor
        if (character != null)
        {
            character.OnResourceUpdated += UpdateResource;
        }
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
        UpdateResource(character.CurrentResource, character.MaxResource);
        character.OnResourceUpdated += UpdateResource;
        character.OnDeath += DestroyResourceBar;
    }

    private void UpdateResource(int currentResource, int maxResource)
    {
        resourceText.text = $"{currentResource}/{maxResource}";
    }

    private void DestroyResourceBar(Character _)
    {
        Destroy(gameObject);
    }
}
