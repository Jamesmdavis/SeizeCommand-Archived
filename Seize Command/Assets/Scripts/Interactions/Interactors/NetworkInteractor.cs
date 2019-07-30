using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Interactions.Interactors
{
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
            Debug.Log("Induce Interact");
            base.Interact();
        }
    }
}