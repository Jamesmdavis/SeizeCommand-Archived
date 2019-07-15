using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Health
{
    public class HealthManager : AbstractHealthManager
    {
        public override void TakeDamage(GameObject sender, float damage)
        {
            ApplyDamage(damage);
        }

        public override void Heal(GameObject sender, float heal)
        {
            health += heal;
        }
    }
}