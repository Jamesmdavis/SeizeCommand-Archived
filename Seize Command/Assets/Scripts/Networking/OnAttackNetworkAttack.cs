using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Utility;
using SeizeCommand.Attack;

namespace SeizeCommand.Networking
{
    public class OnAttackNetworkAttack : AbstractEventSubscriber<AbstractAttack>
    {
        [Header("Class References")]
        private NetworkIdentity networkIdentity;

        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
        }

        private void OnEnable()
        {
            item.OnAttack += NetworkAttack;
        }

        private void OnDisable()
        {
            item.OnAttack -= NetworkAttack;
        }

        private void NetworkAttack()
        {
            networkIdentity.Socket.Emit("attack");
        }
    }
}