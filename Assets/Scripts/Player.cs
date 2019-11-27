using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Character character;
    private static Character target;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public static Character Target
    {
        get => target;
        set
        {
            if (target != value)
            {
                target = value;
                character.Target = target;
            }
        }
    }

    public void ChangeTarget(Character newTarget, bool isLeft)
    {
        Target = newTarget;
        if (isLeft)
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        } else
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public static bool IsDead()
    {
        return character.IsDead();
    }

}
