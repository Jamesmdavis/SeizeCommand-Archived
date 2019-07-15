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
            AbstractDamageSender damageSender = proj.GetComponent<AbstractDamageSender>();
            damageSender.Sender = sender;
        }
    }
}