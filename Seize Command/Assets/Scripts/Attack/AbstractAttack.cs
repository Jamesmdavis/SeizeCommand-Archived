using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Weapon;

namespace SeizeCommand.Attack
{
    public abstract class AbstractAttack : MonoBehaviour
    {
        [SerializeField] protected AbstractGun gun;
        protected abstract void Attack();
    }
}