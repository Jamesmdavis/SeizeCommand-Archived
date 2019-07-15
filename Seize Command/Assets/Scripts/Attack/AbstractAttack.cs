using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Weapon;

namespace SeizeCommand.Attack
{
    public abstract class AbstractAttack : MonoBehaviour
    {
        [Header("Class References")]
        [SerializeField] protected AbstractGun gun;

        public event Action OnAttack;

        protected virtual void FixedUpdate()
        {
            CheckAttack();
        }

        protected virtual void Attack()
        {
            gun.Fire();

            OnAttack();
        }

         protected abstract void CheckAttack();
    }
}