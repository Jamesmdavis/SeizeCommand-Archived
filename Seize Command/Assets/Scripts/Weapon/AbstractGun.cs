using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.DamageSenders;

namespace SeizeCommand.Weapon
{
    public abstract class AbstractGun : MonoBehaviour, IFire
    {   
        [Header("Object References")]
        [SerializeField] protected GameObject projectile;
        [SerializeField] protected Transform projectileSpawnPoint;
        [SerializeField] protected Transform projectileParent;
        [SerializeField] protected GameObject sender;
        [SerializeField] protected Collider2D[] ignoreCollisions;

        public GameObject Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public abstract void Fire();
    }
}