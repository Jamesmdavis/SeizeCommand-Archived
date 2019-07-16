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

        public void InduceInteract()
        {
            if(interactables.Count != 0)
            {
                interactables[0].Interact(this);
            }
        }
    }
}