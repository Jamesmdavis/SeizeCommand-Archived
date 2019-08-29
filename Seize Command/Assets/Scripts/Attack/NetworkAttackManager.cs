using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Attack
{
    public class NetworkAttackManager : AttackManager
    {
        private NetworkIdentity networkIdentity;

        protected override void Start()
        {  
            base.Start();
            networkIdentity = GetComponent<NetworkIdentity>();
        }

        protected override void Update()
        {
            if(networkIdentity)
            {
                if(networkIdentity.IsLocalPlayer)
                {
                    CheckAttack();
                }
            }
        }

        protected override void Attack()
        {
            base.Attack();
            networkIdentity.Socket.Emit("attack");
        }

        public void InduceAttack()
        {
            CurrentWeapon.Fire();
        }
    }
}