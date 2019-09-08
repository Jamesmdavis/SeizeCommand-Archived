using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Weapon;

namespace SeizeCommand.Attack
{
    public class AttackManager : MonoBehaviour
    {
        [Header("Class References")]
        [SerializeField] protected AbstractGun gun;

        public event Action OnAttack;

        public virtual void Attack()
        {
            gun.Fire();
            if(OnAttack != null) OnAttack();
        }
    }
}