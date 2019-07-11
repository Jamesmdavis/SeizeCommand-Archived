using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Networking
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkRotation : MonoBehaviour
    {
        private float oldRotation;
        private NetworkIdentity networkIdentity;
        private Player player;

        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            oldRotation = 0.0f;
            player = new Player();
            player.rotation = new Rotation();
            player.rotation.x = 0;
            player.rotation.y = 0;
            player.rotation.z = 0;

            if(!networkIdentity.IsLocalPlayer) enabled = false;
        }

        private void Update()
        {
            if(networkIdentity.IsLocalPlayer)
            {
                
            }
        }
    }
}