using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Attack
{
    public abstract class AbstractNetworkAttack : AbstractAttack
    {
        [SerializeField] protected NetworkIdentity networkIdentity;

        protected override void FixedUpdate()
        {
            if(networkIdentity)
            {
                if(networkIdentity.IsLocalPlayer)
                {
                    CheckAttack();
                }
            }
        }

        public void InduceAttack()
        {
            gun.Fire();
        }
    }
}