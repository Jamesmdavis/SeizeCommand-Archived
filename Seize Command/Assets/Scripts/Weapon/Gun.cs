using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.DamageSenders;

namespace SeizeCommand.Weapon
{
    public class Gun : AbstractGun
    {
        public event Action<GameObject> OnInstantiateProjectile;

        public override void Fire()
        {
            GameObject proj;

            //If projectileParent variable is not null, instantiate it underneath the parent
            //otherwise instantiate it normally
            proj = projectileParent ? Instantiate(projectile, projectileSpawnPoint.position,
                projectileSpawnPoint.rotation, projectileParent) : Instantiate(projectile,
                projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            Collider2D projColl = proj.GetComponent<Collider2D>();

            for(int i = 0; i < ignoreCollisions.Length; i++)
            {
                Physics2D.IgnoreCollision(projColl, ignoreCollisions[i]);
            }

            DamageSender damageSender = proj.GetComponent<DamageSender>();
            damageSender.Sender = sender;

            if(OnInstantiateProjectile != null) OnInstantiateProjectile(proj);
        }
    }
}