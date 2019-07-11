using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

namespace SeizeCommand.Networking
{
    public class NetworkIdentity : MonoBehaviour
    {
        private string id;
        private bool isLocalPlayer;

        public string ID { get { return id; } }
        public bool IsLocalPlayer { get { return isLocalPlayer; } }

        private SocketIOComponent socket;

        public SocketIOComponent Socket
        {
            get { return socket; }
            set { socket = value; }
        }

        private void Awake()
        {
            isLocalPlayer = false;
        }

        public void SetControllerID(string ID)
        {
            id = ID;
            isLocalPlayer = (NetworkManager.ClientID == ID) ? true : false;
        }

        public void SetSocketReference( SocketIOComponent Socket)
        {
            socket = Socket;
        }
    }
}
