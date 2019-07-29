using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.DamageSenders;

namespace SeizeCommand.Weapon
{
    public class Gun : AbstractGun
    {
        public override void Fire()
        {
            GameObject proj = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            Collider2D[] colls = sender.GetComponentsInChildren<Collider2D>();
            Collider2D projColl = proj.GetComponent<Collider2D>();

            for(int i = 0; i < colls.Length; i++)
            {
                Physics2D.IgnoreCollision(projColl, colls[i]);
            }

            AbstractDamageSender damageSender = proj.GetComponent<AbstractDamageSender>();
            damageSender.Sender = sender;
        }
    }
}