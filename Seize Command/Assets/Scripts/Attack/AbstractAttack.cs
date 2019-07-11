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

        protected virtual void FixedUpdate()
        {
            Attack();
        }

        protected abstract void Attack();
    }
}