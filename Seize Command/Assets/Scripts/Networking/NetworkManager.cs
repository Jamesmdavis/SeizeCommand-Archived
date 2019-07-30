using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

using SeizeCommand.Attack;
using SeizeCommand.Health;
using SeizeCommand.Movement;
using SeizeCommand.Interactions.Interactors;

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
                ClientID = E.data["id"].ToString().Trim('"');

                Debug.LogFormat("Our Client's ID ({0})", ClientID);
            });

            On("spawn", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float x = E.data["position"]["x"].f;
                float y = E.data["position"]["y"].f;
                float rotation = E.data["rotation"].f;

                Vector3 position = new Vector3(x, y, 0);

                GameObject g = Instantiate(player, position, Quaternion.Euler(0, 0, rotation), networkContainer);
                g.name = string.Format("Player ({0})", id);
                NetworkIdentity ni = g.GetComponent<NetworkIdentity>();
                Debug.Log(id);
                ni.SetControllerID(id);
                ni.SetSocketReference(this);
                serverObjects.Add(id, ni);
            });

            On("disconnected", (E) => {
                string id = E.data["id"].ToString().Trim('"');

                GameObject g = serverObjects[id].gameObject;
                Destroy(g); //Remove from game
                serverObjects.Remove(id);   //Remove from memory
            });

            On("updatePosition", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float x = E.data["position"]["x"].f;
                float y = E.data["position"]["y"].f;
                float timeSent = E.data["timeSent"].f;

                NetworkIdentity ni = serverObjects[id];
                NetworkPlayerMovement movement = ni.GetComponent<NetworkPlayerMovement>();
                Vector3 position = new Vector3(x, y, 0);

                movement.CorrectPosition(timeSent, position);
            });

            On("seatUpdatePositionRotation", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float x = E.data["position"]["x"].f;
                float y = E.data["position"]["y"].f;
                float rotation = E.data["rotation"].f;

                NetworkIdentity ni = serverObjects[id];
                Vector3 position = new Vector3(x, y, 0);

                ni.transform.position = position;
                ni.transform.rotation = Quaternion.Euler(0, 0, rotation);
            });

            On("updateRotation", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float rotation = E.data["rotation"].f;

                NetworkIdentity ni = serverObjects[id];
                ni.transform.rotation = Quaternion.Euler(0, 0, rotation);
            });

            On("attack", (E) => {
                string id = E.data["id"].ToString().Trim('"');

                NetworkIdentity ni = serverObjects[id];
                AbstractNetworkAttack attack = ni.GetComponent<AbstractNetworkAttack>();
                
                attack.InduceAttack();
            });

            On("interact", (E) => {
                string id = E.data["id"].ToString().Trim('"');

                NetworkIdentity ni = serverObjects[id];
                NetworkInteractor interactor = ni.GetComponentInChildren<NetworkInteractor>();

                interactor.InduceInteract();
            });

            On("takeDamage", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float damage = E.data["damage"].f;

                NetworkIdentity ni = serverObjects[id];
                HealthNetworkManager healthManager = ni.GetComponent<HealthNetworkManager>();

                healthManager.InduceDamage(damage);
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

    [Serializable]
    public class UpdatePosition
    {
        public float horizontal;
        public float vertical;
        public float speed;
        public float deltaTime;
        public float timeSent;
    }

    // This Serializable Class is used for messages that involve Taking and Leaving Seats
    [Serializable]
    public class SeatUpdatePositionRotation
    {
        public Position position;
        public float rotation;
    }

    [Serializable]
    public class TakeDamage
    {
        public string senderID;
        public string receiverID;
        public float damage;
    }
}
