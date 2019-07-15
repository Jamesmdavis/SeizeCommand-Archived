using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Health
{
    public abstract class AbstractHealthManager : MonoBehaviour, IDamageable, IHealable
    {
        [Header("Data")]
        [SerializeField] protected float health;

        public event Action OnTakeDamage;
        public event Action OnDie;

        public abstract void TakeDamage(GameObject sender, float damage);
        public abstract void Heal(GameObject sender, float heal);

        protected void ApplyDamage(float damage)
        {
            OnTakeDamage();

            health -= damage;

            if(health <= 0)
            {
                Die();
            }
        }

        protected void Die()
        {
            OnDie();

            gameObject.SetActive(false);
        }
    }
}