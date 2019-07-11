using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Movement
{
    public abstract class AbstractNetworkMovement : AbstractMovement
    {
        [Header("Class References")]
        [SerializeField] protected NetworkIdentity networkIdentity;

        protected override void FixedUpdate()
        {
            if(networkIdentity)
            {
                if(networkIdentity.IsLocalPlayer)
                {
                    Move();
                }
            }
        }
    }

}