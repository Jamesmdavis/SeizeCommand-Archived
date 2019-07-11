using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Weapon
{
    public abstract class AbstractGun : MonoBehaviour, IFire
    {
        public abstract void Fire();
    }
}