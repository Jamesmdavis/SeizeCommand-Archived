﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Networking
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkTransform : MonoBehaviour
    {
        private Vector3 oldPosition;
        private NetworkIdentity networkIdentity;
        private Player player;

        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            oldPosition = transform.position;
            player = new Player();
            player.position = new Position();
            player.position.x = 0;
            player.position.y = 0;

            if(!networkIdentity.IsLocalPlayer) enabled = false;
        }

        private void Update()
        {
            if(networkIdentity.IsLocalPlayer)
            {
                if(oldPosition != transform.position)
                {
                    oldPosition = transform.position;
                    SendData();
                }
            }
        }

        private void SendData()
        {
            player.position.x = Mathf.Round(transform.position.x * 1000.0f)/1000.0f;
            player.position.y = Mathf.Round(transform.position.y * 1000.0f)/1000.0f;

            networkIdentity.Socket.Emit("updatePosition", new JSONObject(JsonUtility.ToJson(player)));
        }
    }
}
