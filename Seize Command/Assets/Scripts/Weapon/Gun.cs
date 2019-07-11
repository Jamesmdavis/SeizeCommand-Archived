using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Weapon
{
    public class Gun : AbstractGun
    {
        public override void Fire()
        {
            GameObject proj = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Debug.Log("Fire");
        }
    }
}