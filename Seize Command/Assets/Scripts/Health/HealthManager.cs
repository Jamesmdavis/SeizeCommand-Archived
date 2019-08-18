using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Health
{
    public class HealthManager : MonoBehaviour, IDamageable, IHealable
    {
        [Header("Data")]
        [SerializeField] protected float health;

        public event Action OnTakeDamage;
        public event Action OnDie;
        [SerializeField] private float currentHealth;

        protected virtual void Start()
        {
            currentHealth = health;
        }

        public virtual void TakeDamage(GameObject sender, float damage)
        {
            ApplyDamage(damage);
        }
        public virtual void Heal(GameObject sender, float heal)
        {
            //ApplyHeal();
        }

        protected void ApplyDamage(float damage)
        {
            if(OnTakeDamage != null) OnTakeDamage();

            currentHealth -= damage;

            if(currentHealth <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            if(OnDie != null) OnDie();

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            currentHealth = health;
        }
    }
}