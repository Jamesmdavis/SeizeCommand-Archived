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

        protected virtual void Update()
        {
            CheckAttack();
        }

        protected virtual void Attack()
        {
            gun.Fire();

            if(OnAttack != null) OnAttack();
        }

         protected abstract void CheckAttack();
    }
}