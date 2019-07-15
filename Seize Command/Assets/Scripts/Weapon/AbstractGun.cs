using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.DamageSenders;

namespace SeizeCommand.Weapon
{
    public abstract class AbstractGun : MonoBehaviour, IFire
    {
        [Header("Class References")]
        [SerializeField] protected GameObject projectile;
        

        [Header("Object References")]
        [SerializeField] protected Transform projectileSpawnPoint;
        [SerializeField] protected GameObject sender;

        public abstract void Fire();
    }
}