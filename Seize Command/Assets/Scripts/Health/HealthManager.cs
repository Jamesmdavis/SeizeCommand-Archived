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

            health -= damage;

            if(health <= 0)
            {
                Die();
            }
        }

        protected void Die()
        {
            if(OnDie != null) OnDie();

            gameObject.SetActive(false);
        }
    }
}