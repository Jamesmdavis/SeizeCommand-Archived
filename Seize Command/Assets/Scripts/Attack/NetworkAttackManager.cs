using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;
using SeizeCommand.Utility;

namespace SeizeCommand.Attack
{
    public class NetworkAttackManager : AttackManager
    {
        private NetworkIdentity networkIdentity;

        public override InputManager Controller
        {
            get { return controller; }
            set
            {
                controller = value;
                networkIdentity = controller.GetComponent<NetworkIdentity>();
            }
        }

        protected override void Start()
        {
            base.Start();
            networkIdentity = GetComponent<NetworkIdentity>();
        }

        protected override void Attack()
        {
            base.Attack();
            //networkIdentity.Socket.Emit("attack");
        }

        public void InduceAttack()
        {
            gun.Fire();
        }
    }
}