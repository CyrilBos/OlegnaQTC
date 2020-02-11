using System.Collections;
using System.Collections.Generic;
using Skills;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private string skillName;
    
    private CooldownSkill skill;

    private Character player;

    private Text text;
    private Button button;
    private Image cooldownOverlay;

    private void Start()
    {
        button = GetComponent<Button>();
        cooldownOverlay = transform.Find("CooldownOverlay").GetComponent<Image>();
        
        player = CombatManager.Player.GetComponent<Character>();
        skill = CombatManager.Player.GetSkillByName(skillName);
        player.OnGlobalCooldown += HandleGlobalCooldown;
        
        // TODO: button.onClick.AddListener(skill.UseSkill);

        
        // Set the skill button with the skill name
        transform.Find("ButtonText").GetComponent<Text>().text = skill.Name;

        text = transform.Find("CooldownText").GetComponent<Text>();
        text.text = skill.Cooldown.ToString() + 's';

        skill.OnCooldownUpdate += UpdateCooldownText;
    }

    private void OnDisable()
    {
        skill.OnCooldownUpdate -= UpdateCooldownText;
        player.OnGlobalCooldown -= HandleGlobalCooldown;
    }

    private void UpdateCooldownText(float cooldown)
    {
        displayCurrentCooldown(cooldown, skill.Cooldown);
    }

    private void HandleGlobalCooldown(float globalCooldown)
    {
        if (globalCooldown > skill.CurrentCooldown)
        {
            displayCurrentCooldown(globalCooldown, Character.GlobalCooldownDuration, true);
        } else
        {
            displayCurrentCooldown(skill.CurrentCooldown, skill.Cooldown);
        }
    }

    private void displayCurrentCooldown(float currentCooldown, float totalCooldown, bool disableCooldownText = false)
    {
        if (currentCooldown <= 0)
        {
            text.enabled = false;
            button.interactable = true;
        }
        else
        {
            if (disableCooldownText)
                text.enabled = false;
            else 
                text.enabled = true;
            button.interactable = false;
            text.text = currentCooldown.ToString("0.00") + 's';
            cooldownOverlay.fillAmount = (currentCooldown / totalCooldown);
        }
    }



}
