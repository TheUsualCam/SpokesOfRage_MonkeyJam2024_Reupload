using UnityEngine;

namespace MonkeyJam._Scripts.Core_Scripts.Entities
{
    public class EntityData : MonoBehaviour
    {
        public int MaxHealth { get; } = 100;
        public int CurrentHealth { get; private set; }
        
        
        public bool IsTakingDamage { get; private set; }

        void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            IsTakingDamage = true;
        }

        public void ResetIsTakingDamage()
        {
            IsTakingDamage = false;
        }

        public bool IsEntityDead() => CurrentHealth <= 0;
    }
}