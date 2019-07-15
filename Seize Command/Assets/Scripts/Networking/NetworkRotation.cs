using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Aim;

namespace SeizeCommand.Networking
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkRotation : MonoBehaviour
    {
        private float oldRotation;
        private NetworkIdentity networkIdentity;
        private AbstractAim abstractAim;
        private Player player;

        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            abstractAim = GetComponent<AbstractAim>();
            oldRotation = 0.0f;
            player = new Player();
            player.rotation = 0.0f;
            if(!networkIdentity.IsLocalPlayer) enabled = false;
        }

        private void Update()
        {
            if(networkIdentity.IsLocalPlayer)
            {
                if(oldRotation != abstractAim.Rotation)
                {
                    oldRotation = abstractAim.Rotation;
                    SendData();
                }
            }
        }

        private void SendData()
        {
            player.rotation = oldRotation;
            networkIdentity.Socket.Emit("updateRotation", new JSONObject(JsonUtility.ToJson(player)));
        }
    }
}