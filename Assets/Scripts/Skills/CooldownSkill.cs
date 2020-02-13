using System;
using UnityEngine;

namespace Skills
{
    public class SkillMissedEventArgs : EventArgs
    {
        public Vector3 Position { get; }

        public SkillMissedEventArgs(Vector3 position)
        {
            Position = position;
        }
    }
    
    public class CooldownSkill : MonoBehaviour
    {
        [SerializeField]
        private new String name;

        [SerializeField]
        private float cooldown = 1;

        [SerializeField]
        private int resourceCost;

        [SerializeField]
        private SkillEffect[] effects;
            
        private Character user;

        [SerializeField]
        private String animationStateName; 

        private float currentCooldown = 0f;

        public string Name { get => name; }
        public float Cooldown { get => cooldown; }
        public float CurrentCooldown { get => currentCooldown; }

        public Action<float> OnCooldownUpdate;
        public static EventHandler<SkillMissedEventArgs> TargetDodged;

        private void Awake()
        {
            user = GetComponent<Character>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
                if (OnCooldownUpdate != null)
                    OnCooldownUpdate(currentCooldown);
            }
        }

        public bool IsUsable()
        {
            return currentCooldown <= 0 && user.CurrentResource >= resourceCost && !user.IsDead() && !user.Target.IsDead() && !user.IsInGlobalCooldown() && !user.IsDodging();
        }

        public void Use()
        {
            if (!IsUsable()) return;
        
            user.SpendResource(resourceCost);
            user.ChangeAnimationState(animationStateName);
            user.TriggerGlobalCooldown();
            currentCooldown = cooldown;
        }

        public void ApplySkillEffect(String skillName)
        {
            if (name != skillName) return;
            
            if (user.Target.DodgesSkill(this))
            {
                TargetDodged(this, new SkillMissedEventArgs(user.Target.gameObject.transform.position));
                return;
            }
            
            foreach (SkillEffect effect in effects)
            {
                effect.ApplyEffect(user.Target);
            }
            
            // triggers hurt animation
            user.Target.GetHit();
        }
    }
}
