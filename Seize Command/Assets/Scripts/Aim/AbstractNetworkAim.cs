using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Aim
{
    public abstract class AbstractNetworkAim : AbstractAim
    {
        [Header("Class References")]
        [SerializeField] protected NetworkIdentity networkIdentity;

        protected override void FixedUpdate()
        {
            if(networkIdentity)
            {
                if(networkIdentity.IsLocalPlayer)
                {
                    Aim();
                }
            }
        }
    }
}