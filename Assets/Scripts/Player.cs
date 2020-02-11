using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Character s_character;
    private static Character s_target;

    private void Awake()
    {
        s_character = GetComponent<Character>();
        
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
}
