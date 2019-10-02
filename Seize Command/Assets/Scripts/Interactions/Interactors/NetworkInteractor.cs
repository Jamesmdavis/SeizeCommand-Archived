using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;
using SeizeCommand.Interactions.Interactables;

namespace SeizeCommand.Interactions.Interactors
{
    //Note: Might Change the system in the future to where the server first verifies whether
    //or not an interaction can occur and then the client performs the interact
    public class NetworkInteractor : Interactor
    {
        [SerializeField] private NetworkIdentity networkIdentity;
        
        protected override void Interact(IInteractable interactable)
        {
            base.Interact(interactable);
            SendData(interactable);
        }

        public void RPCInteract(INetworkInteractable interactable)
        {
            interactable.RPCInteract(this);
            CallOnInteract();
        }

        private void SendData(IInteractable interactable)
        {
            NetworkIdentity receiverNetworkIdentity = 
                (interactable as MonoBehaviour).GetComponent<NetworkIdentity>();

            SendReceivePackage package = new SendReceivePackage();
            package.senderID = networkIdentity.ID;
            package.receiverID = receiverNetworkIdentity.ID;

            networkIdentity.Socket.Emit("interact", new JSONObject(JsonUtility.ToJson(package)));
        }
    }
}