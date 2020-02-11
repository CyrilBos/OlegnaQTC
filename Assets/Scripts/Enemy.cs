using System.Linq;
using Skills;
using UnityEngine;

public enum EnemyState
{
    Fighting,
    Waiting,
    Moving
}

public class Enemy : MonoBehaviour
{
    public Character Character { get; private set; }

    private EnemyState _state = EnemyState.Waiting;
    
    private Vector2 _targetPosition;

    private static float MovingSpeed = 0.1f;
    private static float MoveToFightPositionDelta = 0.001f;

    private void Awake()
    {
        Character = GetComponent<Character>();
        Character.Target = GameObject.Find("Player").GetComponent<Character>();
        Character.OnDeath += RemoveBody;
    }

    private void OnDisable()
    {
        Character.OnDeath -= RemoveBody;
    }

    private void Update()
    {
        if (Player.IsDead() || Character.IsDead())
            return;

        switch (_state)
        {
            case EnemyState.Moving when Vector2.Distance(transform.position, _targetPosition) < Enemy.MoveToFightPositionDelta:
                StartFighting();
                break;
            case EnemyState.Moving:
                transform.position = Vector2.MoveTowards(transform.position, _targetPosition, Enemy.MovingSpeed);
                break;
            case EnemyState.Fighting when !Character.IsInGlobalCooldown():
            {
                var skillToUse = ChooseSkill();
                if (skillToUse != null)
                {
                    skillToUse.Use();
                }

                break;
            }
        }
    }

    public void StartFighting()
    {
        _state = EnemyState.Fighting;
    }

    public void GoInFight(Vector2 position)
    {
         _targetPosition = position;
        _state = EnemyState.Moving;
    }

    private CooldownSkill ChooseSkill()
    {
        return Character.Attacks.FirstOrDefault(attack => attack.IsUsable());
    }

    private void RemoveBody(Character _)
    {
        Destroy(gameObject);
    }
}
