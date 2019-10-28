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
        target = GameObject.Find("EnemyRight").GetComponent<Character>();
        character.Target = target;
    }

    public static Character Target
    {
        get => target;
        set
        {
            if (target != value)
            {
                Debug.Log($"new target {value}");
                target = value;
                character.Target = target;
                character.gameObject.transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }

    public static bool IsDead()
    {
        return character.IsDead();
    }

    public static void EnemyDied(Character enemy)
    {
        if (target == enemy)
        {
            GameObject newTarget = GameObject.Find("EnemyLeft");
            if (target.gameObject == newTarget)
            {
                newTarget = GameObject.Find("EnemyRight");
                if (newTarget != null)
                {
                    Target = newTarget.GetComponent<Character>();
                }
                else
                {
                    Debug.Log("Both enemies dead");
                }
            }
            else if (newTarget != null)
            {
                Target = newTarget.GetComponent<Character>();
            }
        }
    }
}
