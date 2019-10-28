using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField]
    private AttackSkill skill;

    private Text text;
    private Button button;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        text = transform.Find("CooldownText").GetComponent<Text>();
        skill.OnCooldownUpdate += UpdateCooldownText;
    }

    private void OnDisable()
    {
        skill.OnCooldownUpdate -= UpdateCooldownText;
    }

    private void UpdateCooldownText(float cooldown)
    {
        if (cooldown <= 0)
        {
            text.enabled = false;
            button.interactable = true;
        }
        else
        {
            text.enabled = true;
            button.interactable = false;
            text.text = cooldown.ToString("0.00") + 's';
        }
    }
}
