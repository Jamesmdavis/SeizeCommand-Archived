﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

namespace SeizeCommand.Networking
{
    public class NetworkManager : SocketIOComponent
    {
        [Header("Network Client")]
        [SerializeField]
        private Transform networkContainer;

        private Dictionary<string, GameObject> serverObjects;

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
            serverObjects = new Dictionary<string, GameObject>();
        }

        private void SetupEvents()
        {
            On("open", (E) => {
                Debug.Log("Connection Made to the Server");
            });

            On("register", (E) => {
                string id = E.data["id"].ToString();

                Debug.LogFormat("Our Client's ID ({0})", id);
            });

            On("spawn", (E) => {
                string id = E.data["id"].ToString();

                GameObject g = new GameObject("Server ID: " + id);
                g.transform.SetParent(networkContainer);
                serverObjects.Add(id, g);
            });

            On("disconnected", (E) => {
                string id = E.data["id"].ToString();

                GameObject g = serverObjects[id];
                Destroy(g);
                serverObjects.Remove(id);
            });
        }
    }
}
