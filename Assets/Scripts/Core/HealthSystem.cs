using System;
using UnityEngine;
using UnityEngine.Events;

namespace PufferGo.Core
{
    public class HealthSystem : MonoBehaviour, IDamagble, IHealble, IHealthShowable
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _curHealth;
        [SerializeField] private float _saveTime;
        [SerializeField] private bool _isInvincible = false;
        [SerializeField] private UnityEvent _onDeathUnityEvent;
        [SerializeField] private UnityEvent _onDamageTakeUnityEvent;
        public float MaxHealth { get => _maxHealth; }
        public float CurrentHealth { get => _curHealth; }

        public event Action<float> OnHealthChanged;
        public event Action<float, bool> OnHealthDamaged;
        public event Action<float> OnInvinsibleState;
        public event Action OnDeathEvent;

        private float lastDmgTime;

        public void Damage(float value, bool isCrit = false)
        {
            if (Time.time - lastDmgTime < _saveTime || _isInvincible || _curHealth <= 0) return;

            _curHealth = Mathf.Clamp(_curHealth - value, 0, _maxHealth);
            OnHealthDamaged?.Invoke(value, isCrit);
            OnHealthChanged?.Invoke(_curHealth);
            OnInvinsibleState?.Invoke(_saveTime);
            _onDamageTakeUnityEvent?.Invoke();
            lastDmgTime = Time.time;

            if (_curHealth == 0)
            {
                Death();
            }
        }

        public void Heal(float value)
        {
            _curHealth = Mathf.Clamp(_curHealth + value, 0, _maxHealth);
            OnHealthChanged?.Invoke(_curHealth);
        }

        public void SetInvincibility(bool value)
        {
            _isInvincible = value;
        }

        private void Death()
        {
            OnDeathEvent?.Invoke();
            _onDeathUnityEvent?.Invoke();
        }
    }
}