using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Weapon;
using SeizeCommand.Utility;

namespace SeizeCommand.Attack
{
    public class AttackManager : MonoBehaviour
    {
        [Header("Class References")]
        [SerializeField] protected AbstractGun gun;

        protected InputManager controller;

        public virtual InputManager Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        public event Action OnAttack;

        protected virtual void Start()
        {
            controller = GetComponent<InputManager>();
        }

        public virtual void Attack()
        {
            gun.Fire();
            if(OnAttack != null) OnAttack();
        }
    }
}