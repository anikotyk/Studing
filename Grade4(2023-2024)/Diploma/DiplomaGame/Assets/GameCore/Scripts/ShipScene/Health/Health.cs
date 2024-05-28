using NaughtyAttributes;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.ShipScene
{
    [RequireComponent(typeof(Collider))]
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private HealthTargetType _healthTargetType;

        public HealthTargetType targetType => _healthTargetType;

        public int _damage = 0;
        public bool isDied { get; private set; } =  false;
        public int maxHealth => _maxHealth;
        public int currentHealth => Mathf.Clamp(_maxHealth - _damage, 0, _maxHealth);
        public TheSignal revived { get; } = new();
        public TheSignal<int> damaged { get; } = new();
        public TheSignal<int> healed { get; } = new();
        public TheSignal healthChanged { get; } = new();
        public TheSignal died { get; } = new();

        [Button()]
        public void Revive()
        {
            isDied = false;
            revived.Dispatch();
            HealToMax();
        }

        public void HealToMax()
        {
            Heal(_damage);
            healthChanged.Dispatch();;
        }

        [Button()]
        public void Kill()
        {
            Damage(currentHealth);
        }

        public bool CanApplyDamage()
        {
            return isDied == false  && currentHealth > 0;
        }
        
        public bool CanApplyDamage(int damage)
        {
            return CanApplyDamage() && damage > 0;
        }
        
        public void Damage(int damage)
        {
            damage = Mathf.Min(damage, currentHealth);
            if (damage <= 0)
            {
                return;
            }
            _damage += damage;
            damaged.Dispatch(damage);
            if (currentHealth <= 0)
            {
                if(isDied)
                    return;
                isDied = true;
                died.Dispatch();
            }
            healthChanged.Dispatch();
        }

        public void Heal(int heal)
        {
            heal = Mathf.Max(_damage, heal);
            if (heal <= 0)
            {
                return;
            }
            _damage -= heal;
            healed.Dispatch(heal);
            healthChanged.Dispatch();
        }
        
        
    }
}