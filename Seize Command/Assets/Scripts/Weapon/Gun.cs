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
            Transform projectileParent = sender.transform.parent.parent;
            GameObject proj = Instantiate(projectile, projectileSpawnPoint.position,
                projectileSpawnPoint.rotation, projectileParent);

            Collider2D[] colls = sender.GetComponentsInChildren<Collider2D>();
            Collider2D projColl = proj.GetComponent<Collider2D>();

            for(int i = 0; i < colls.Length; i++)
            {
                Physics2D.IgnoreCollision(projColl, colls[i]);
            }

            DamageSender damageSender = proj.GetComponent<DamageSender>();
            damageSender.Sender = sender;

            if(OnInstantiateProjectile != null) OnInstantiateProjectile(proj);
        }
    }
}