using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

using SeizeCommand.Attack;
using SeizeCommand.Health;
using SeizeCommand.Movement;
using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Interactions.Interactables;
using SeizeCommand.Utility;
using SeizeCommand.Referencing;
using SeizeCommand.Scriptable;

namespace SeizeCommand.Networking
{
    public class NetworkManager : SocketIOComponent
    {
        [Header("Network Client")]
        [SerializeField] private Transform networkContainer;

        [Header("Object References")]
        [SerializeField] private ServerObjects serverSpawnables;

        private Dictionary<string, NetworkIdentity> serverObjects;

        public static ClientIDS LocalPlayers;

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

        public void Play(InputField inputField)
        {
            string ipAddress = inputField.text;
            url = "ws://" + ipAddress + ":52300/socket.io/?EIO=4&transport=websocket";
            CreateWebSocket();
            Connect();
        }

        public void DeactivateMainMenu(GameObject maniMenuPanel)
        {
            maniMenuPanel.SetActive(false);
        }

        public void Respawn()
        {
            Emit("respawn");
        }

        private void Initialize()
        {
            serverObjects = new Dictionary<string, NetworkIdentity>();
            LocalPlayers.id = new string[2];
        }

        private void SetupEvents()
        {
            On("open", (E) => {
                Debug.Log("Connection Made to the Server");
            });

            On("register", (E) => {
                LocalPlayers.id[0] = E.data["id1"].ToString().Trim('"');
                LocalPlayers.id[1] = E.data["id2"].ToString().Trim('"');
            });

            On("respawn", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float x = E.data["position"]["x"].f;
                float y = E.data["position"]["y"].f;
                float rotation = E.data["rotation"].f;

                GameObject g = serverObjects[id].gameObject;
                g.SetActive(true);

                g.transform.position = new Vector3(x, y, 0);
                g.transform.rotation = Quaternion.Euler(0, 0, rotation);
            });

            On("disconnected", (E) => {
                string id = E.data["id"].ToString().Trim('"');

                GameObject g = serverObjects[id].gameObject;
                Destroy(g); //Remove from game
                serverObjects.Remove(id);   //Remove from memory
            });

            On("changePosition", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float x = E.data["vector2"]["x"].f;
                float y = E.data["vector2"]["y"].f;

                NetworkIdentity ni = serverObjects[id];
                Vector3 position = new Vector3(x, y, 0);
                ni.transform.localPosition = position;
            });

            On("changeVelocity", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float x = E.data["vector2"]["x"].f;
                float y = E.data["vector2"]["y"].f;

                NetworkIdentity ni = serverObjects[id];
                Vector2 velocity = new Vector2(x, y);
                Rigidbody2D rb = ni.GetComponent<Rigidbody2D>();
                rb.velocity = velocity;
            });

            On("changeRotation", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float rotation = E.data["rotation"].f;

                NetworkIdentity ni = serverObjects[id];
                ni.transform.localRotation = Quaternion.Euler(0, 0, rotation);
            });

            On("seatMove", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float x = E.data["position"]["x"].f;
                float y = E.data["position"]["y"].f;
                float rotation = E.data["rotation"].f;

                NetworkIdentity ni = serverObjects[id];
                Vector3 position = new Vector3(x, y, 0);

                ni.transform.localPosition = position;
                ni.transform.rotation = Quaternion.Euler(0, 0, rotation);
            });

            On("attack", (E) => {
                string id = E.data["id"].ToString().Trim('"');

                NetworkIdentity ni = serverObjects[id];
                NetworkAttackManager attack = ni.GetComponent<NetworkAttackManager>();
                
                attack.InduceAttack();
            });

            On("interact", (E) => {
                string senderID = E.data["senderID"].ToString().Trim('"');
                string receiverID = E.data["receiverID"].ToString().Trim('"');

                NetworkIdentity senderNI = serverObjects[senderID];
                NetworkIdentity receiverNI = serverObjects[receiverID];
                NetworkInteractor senderInteractor = senderNI.GetComponentInChildren<NetworkInteractor>();
                IInteractable receiverInteractable = receiverNI.GetComponent<IInteractable>();
                
                senderInteractor.RPCInteract(receiverInteractable);
            });

            On("takeDamage", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                float damage = E.data["damage"].f;

                NetworkIdentity ni = serverObjects[id];
                NetworkHealthManager healthManager = ni.GetComponent<NetworkHealthManager>();

                healthManager.InduceDamage(damage);
            });

            On("serverSpawn", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                string name = E.data["name"].str;
                float x = E.data["position"]["x"].f;
                float y = E.data["position"]["y"].f;
                float rotation = E.data["rotation"].f;

                Vector2 positionData = new Vector2(x, y);
                Quaternion rotationData = Quaternion.Euler(0, 0, rotation);

                if(!serverObjects.ContainsKey(id))
                {
                    ServerObjectData sod = serverSpawnables.GetObjectByName(name);
                    GameObject spawnedObject = Instantiate(sod.prefab, positionData, rotationData,
                        networkContainer);

                    NetworkIdentity ni = spawnedObject.GetComponent<NetworkIdentity>();
                    ni.SetControllerID(id);
                    ni.SetSocketReference(this);

                    serverObjects.Add(id, ni);
                }
            });

            On("serverSpawnMirroredPair", (E) => {
                string spawn1ID = E.data["spawn1ID"].ToString().Trim('"');
                string spawn2ID = E.data["spawn2ID"].ToString().Trim('"');
                string parentID = E.data["parentID"].ToString().Trim('"');
                string spawn1Name = E.data["spawn1Name"].str;
                string spawn2Name = E.data["spawn2Name"].str;
                float x = E.data["position"]["x"].f;
                float y = E.data["position"]["y"].f;
                float rotation = E.data["rotation"].f;

                Vector2 positionData = new Vector2(x, y);
                Quaternion rotationData = Quaternion.Euler(0, 0, rotation);

                if(!serverObjects.ContainsKey(spawn1ID) && !serverObjects.ContainsKey(spawn2ID))
                {
                    ServerObjectData sod1 = serverSpawnables.GetObjectByName(spawn1Name);
                    ServerObjectData sod2 = serverSpawnables.GetObjectByName(spawn2Name);

                    NetworkIdentity parentNI = serverObjects[parentID];
                    References<GameObject> parentReferences = parentNI.GetComponent<References<GameObject>>();
                    Transform staticObjectParent = parentReferences.GetReferenceByName("Static Deck").transform;
                    Transform dynamicObjectParent = parentReferences.GetReferenceByName("Dynamic Deck").transform;

                    GameObject spawnedObject1 = staticObjectParent ? Instantiate(sod1.prefab, positionData,
                        rotationData, staticObjectParent) : Instantiate(sod1.prefab, positionData,
                        rotationData, networkContainer);

                    GameObject spawnedObject2 = dynamicObjectParent ? Instantiate(sod2.prefab, positionData,
                        rotationData, dynamicObjectParent) : Instantiate(sod2.prefab, positionData,
                        rotationData, networkContainer);

                    NetworkIdentity networkIdentity1 = spawnedObject1.GetComponent<NetworkIdentity>();
                    NetworkIdentity networkIdentity2 = spawnedObject2.GetComponent<NetworkIdentity>();
                    networkIdentity1.SetControllerID(spawn1ID);
                    networkIdentity2.SetControllerID(spawn2ID);
                    networkIdentity1.SetSocketReference(this);
                    networkIdentity2.SetSocketReference(this);

                    serverObjects.Add(spawn1ID, networkIdentity1);
                    serverObjects.Add(spawn2ID, networkIdentity2);

                    References<GameObject> object1Refereces = spawnedObject1
                        .GetComponent<References<GameObject>>();
                    References<GameObject> object2Refereces = spawnedObject2
                        .GetComponent<References<GameObject>>();

                    object1Refereces.AddReference("Mirror Target", spawnedObject2);
                    object2Refereces.AddReference("Mirror Target", spawnedObject1);

                    GameObject staticShip = UtilityMethods.FindGameObjectInParentByName
                        ("Static Space Ship", spawnedObject1.transform);
                    GameObject dynamicShip = UtilityMethods.FindGameObjectInParentByName
                        ("Dynamic Space Ship", spawnedObject2.transform);

                    object1Refereces.AddReference("Static Ship", staticShip);
                    object2Refereces.AddReference("Dynamic Ship", dynamicShip);

                    MirrorTransform mirrorTransform1 = spawnedObject1.GetComponent<MirrorTransform>();
                    MirrorTransform mirrorTransform2 = spawnedObject2.GetComponent<MirrorTransform>();
                    mirrorTransform1.StartMirroring();
                    mirrorTransform2.StartMirroring();

                    if(spawn1Name == "Player")
                    {
                        if(networkIdentity1.IsLocalPlayer)
                        {
                            //Camera follow the Local Player.  This sets up the reference
                            Camera mainCamera = Camera.main;
                            References<Transform> camReferences = 
                                mainCamera.GetComponent<References<Transform>>();

                            camReferences.AddReference("Player Mirror", spawnedObject2.transform);
                        }
                        else
                        {
                            //Non Local Players don't really need Collision Detection
                            CircleCollider2D coll = spawnedObject1.GetComponent<CircleCollider2D>();
                            coll.isTrigger = true;
                        }

                        //Sets the name of the Player to the ID
                        spawnedObject1.name = string.Format("Player ({0})", spawn1Name);
                        spawnedObject2.name = string.Format("Player ({0})", spawn2Name);
                    }
                }
            });

            On("serverDespawn", (E) => {
                string id = E.data["id"].ToString().Trim('"');
            });
        }
    }

    [Serializable]
    public class Player
    {
        public string id;
        public Vector2Data position;
        public float rotation;
    }

    [Serializable]
    public class SpawnPackage
    {
        public string name;
        public Vector2Data position;
        public float rotation;
    }

    [Serializable]
    public class Vector2Data
    {
        public float x;
        public float y;
    }

    [Serializable]
    public class Vector2Package
    {
        public string id;
        public Vector2Data vector2;
    }

    [Serializable]
    public class RotationPackage
    {
        public string id;
        public float rotation;
    }

    [Serializable]
    public class Damage
    {
        public string senderID;
        public string receiverID;
        public float damage;
    }

    [Serializable]
    public class SendReceivePackage
    {
        public string senderID;
        public string receiverID;
    }

    public class ClientIDS
    {
        public string[] id;
    }
}
