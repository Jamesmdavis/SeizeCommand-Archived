using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Interactions.Interactors
{
    //Note: Might Change the system in the future to where the server first verifies whether
    //or not an interaction can occur and then the client performs the interact
    public class NetworkInteractor : Interactor
    {
        [SerializeField] private NetworkIdentity networkIdentity;

        protected override void Update()
        {
            if(networkIdentity)
            {
                if(networkIdentity.IsLocalPlayer)
                {
                    CheckInteract();
                }
            }
        }
        
        protected override void Interact()
        {
            base.Interact();
            networkIdentity.Socket.Emit("interact");
        }

        public void InduceInteract()
        {
            //For Future Use
            //base.Interact();
        }
    }
}