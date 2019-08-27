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

        public AbstractGun CurrentWeapon
        {
            get { return currentWeapon; }
            set { currentWeapon = value; }
        }

        private AbstractGun currentWeapon;

        public event Action OnAttack;

        protected virtual void Start()
        {
            currentWeapon = gun;
        }

        protected virtual void Update()
        {
            CheckAttack();
        }

        protected virtual void Attack()
        {
            currentWeapon.Fire();

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
            currentWeapon = gun;
        }
    }
}