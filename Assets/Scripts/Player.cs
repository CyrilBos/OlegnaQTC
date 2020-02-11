using System;
using System.Collections;
using System.Collections.Generic;
using Skills;
using UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CooldownSkill[] _attacks;

    private static Character s_character;
    private static Character s_target;

    private void Awake()
    {
        s_character = GetComponent<Character>();
        _attacks = GetComponents<CooldownSkill>();
        DraggableObject draggable = GetComponent<DraggableObject>();
        draggable.draggedAway += s_character.Dodge;
        s_character.StoppedDodging += draggable.ResetPosition;
    }

    public void ChangeTarget(Character newTarget, bool isLeft)
    {
        if (s_target == newTarget) return;
        if (newTarget == s_character) return;
        
        s_target = newTarget;
        s_character.Target = s_target;
        
        transform.eulerAngles = isLeft ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);
    }

    public static bool IsDead()
    {
        return s_character.IsDead();
    }

    public void UseSkill(String skillName)
    {
        GetSkillByName(skillName).Use();
    }

    public CooldownSkill GetSkillByName(string skillName)
    {                // TODO replace by a Map<String, Skill> ?

        for (var i = 0; i < _attacks.Length; i++)
        {
            var skill = _attacks[i];
            if (skill.Name == skillName)
            {
                return skill;
            }
        }

        return null;
    }
}
