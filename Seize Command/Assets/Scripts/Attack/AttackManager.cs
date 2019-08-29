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

        public AbstractGun CurrentWeapon { get; set; }

        public event Action OnAttack;

        protected virtual void Start()
        {
            CurrentWeapon = gun;
        }

        protected virtual void Update()
        {
            CheckAttack();
        }

        protected virtual void Attack()
        {
            CurrentWeapon.Fire();
            if(OnAttack != null) OnAttack();
        }

        protected void CheckAttack()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }

        public void DefaultWeapon()
        {
            CurrentWeapon = gun;
        }
    }
}