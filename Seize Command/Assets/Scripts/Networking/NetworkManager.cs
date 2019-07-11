using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

using SeizeCommand.Attack;

namespace SeizeCommand.Networking
{
    public class NetworkManager : SocketIOComponent
    {
        [Header("Network Client")]
        [SerializeField] private Transform networkContainer;

        [SerializeField] private GameObject player;

        private Dictionary<string, NetworkIdentity> serverObjects;

        public static string ClientID { get; private set; }

        public override void Start()
        {
            base.Start();
            Initialize();
            SetupEvents();
        }

        public override void Update()
        {
            base.Update();
        }

        private void Initialize()
        {
            serverObjects = new Dictionary<string, NetworkIdentity>();
        }

        private void SetupEvents()
        {
            On("open", (E) => {
                Debug.Log("Connection Made to the Server");
            });

            On("register", (E) => {
                ClientID = E.data["id"].ToString();

                Debug.LogFormat("Our Client's ID ({0})", ClientID);
            });

            On("spawn", (E) => {
                string id = E.data["id"].ToString();

                GameObject g = Instantiate(player, networkContainer);
                g.name = string.Format("Player ({0})", id);
                NetworkIdentity ni = g.GetComponent<NetworkIdentity>();
                ni.SetControllerID(id);
                ni.SetSocketReference(this);
                serverObjects.Add(id, ni);
            });

            On("disconnected", (E) => {
                string id = E.data["id"].ToString();

                GameObject g = serverObjects[id].gameObject;
                Destroy(g); //Remove from game
                serverObjects.Remove(id);   //Remove from memory
            });

            On("updatePosition", (E) => {
                string id = E.data["id"].ToString();
                float x = E.data["position"]["x"].f;
                float y = E.data["position"]["y"].f;

                NetworkIdentity ni = serverObjects[id];
                ni.transform.position = new Vector3(x, y, 0);
            });

            On("updateRotation", (E) => {
                string id = E.data["id"].ToString();
                float rotation = E.data["rotation"].f;

                NetworkIdentity ni = serverObjects[id];
                ni.transform.rotation = Quaternion.Euler(0, 0, rotation);
            });

            On("attack", (E) => {
                string id = E.data["id"].ToString();

                NetworkIdentity ni = serverObjects[id];
                AbstractNetworkAttack attack = ni.GetComponent<AbstractNetworkAttack>();
                
                attack.InduceAttack();
            });
        }
    }

    [Serializable]
    public class Player
    {
        public string id;
        public Position position;
        public float rotation;
    }

    [Serializable]
    public class Position
    {
        public float x;
        public float y;
    }
}
