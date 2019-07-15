using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Health;

namespace SeizeCommand.DamageSenders
{
    public class ProjectileDamageSender : AbstractDamageSender
    {
        protected override void SendDamage(IDamageable damageable)
        {
            damageable.TakeDamage(Sender, damage);
        }
    }
}