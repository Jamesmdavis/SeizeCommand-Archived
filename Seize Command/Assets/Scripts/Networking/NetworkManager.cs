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
                string id = E.data["id"].ToString().Trim('"');

                NetworkIdentity ni = serverObjects[id];
                NetworkInteractor interactor = ni.GetComponentInChildren<NetworkInteractor>();

                interactor.InduceInteract();
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

                    if(name == "Projectile")
                    {
                        string mirrorID = E.data["mirrorID"].ToString().Trim('"');
                        string mirrorName = E.data["mirrorName"].str;

                        ServerObjectData sod2 = serverSpawnables.GetObjectByName(mirrorName);
                        GameObject spawnedObject2 = Instantiate(sod2.prefab,
                            positionData, rotationData, networkContainer);
                    }
                    if(name == "Player")
                    {
                        string mirrorID = E.data["mirrorID"].ToString().Trim('"');
                        string mirrorName = E.data["mirrorName"].str;

                        //These next few lines instantiate the Mirrored Player
                        //This is the player the camera follows and is located on the Dynamic Space Ship
                        ServerObjectData sod2 = serverSpawnables.GetObjectByName(mirrorName);
                        GameObject spawnedObject2 = Instantiate(sod2.prefab, 
                            positionData, rotationData, networkContainer);




                        //Set the Player to the correct position in the ship hierarchy
                        References<Transform> parentReferences = GetComponentInParent<References<Transform>>();
                        spawnedObject.transform.parent = parentReferences.GetReferenceByName("Static Ship");
                        spawnedObject2.transform.parent = parentReferences.GetReferenceByName("Dynamic Ship");



                        
                        //These Reference scripts allow the two versions of the player to communicate
                        //These sets of lines sets up a transform reference to each of the two Players
                        References<Transform> transformReferences = 
                            spawnedObject.GetComponent<References<Transform>>();
                        References<Transform> mirrorTransformReferences = 
                            spawnedObject2.GetComponent<References<Transform>>();    
                        transformReferences.AddReference("Mirror Target", spawnedObject2.transform);
                        mirrorTransformReferences.AddReference("Mirror Target", spawnedObject.transform);

                       //These sets of lines sets up a gameobject reference to each of the two Players
                        References<GameObject> gameObjectReferences =
                            spawnedObject.GetComponent<References<GameObject>>();
                        References<GameObject> mirrorGameObjectReferences =
                            spawnedObject2.GetComponent<References<GameObject>>();
                        gameObjectReferences.AddReference("Other Player", spawnedObject2);
                        mirrorGameObjectReferences.AddReference("Other Player", spawnedObject);

                        //These sets of lines sets up a gameobject reference to the ships
                        //The Static Player creates a reference to the Static Space Ship
                        //The Dynamic Player creates a reference to the Dynamic Space Ship
                        GameObject staticSpaceShip = UtilityMethods.FindGameObjectInParentByName
                            ("Static Space Ship", spawnedObject.transform);
                        gameObjectReferences.AddReference("Static Space Ship", staticSpaceShip);
                        GameObject dynamicSpaceShip = UtilityMethods.FindGameObjectInParentByName
                            ("Dynamic Space Ship", spawnedObject2.transform);
                        mirrorGameObjectReferences.AddReference("Dynamic Space Ship", dynamicSpaceShip);




                        //Begin Mirroring the Transforms
                        MirrorTransform mirrorTransform1 = spawnedObject.GetComponent<MirrorTransform>();
                        MirrorTransform mirrorTransform2 = spawnedObject2.GetComponent<MirrorTransform>();

                        mirrorTransform1.StartMirroring();
                        mirrorTransform2.StartMirroring();




                        //These next few lines set up the localPlayer checks and ID checks for the mirrored Player
                        //These checks make sure that the local player cannot control other players
                        NetworkIdentity playerMirrorNI = spawnedObject2.GetComponent<NetworkIdentity>();
                        playerMirrorNI.SetControllerID(mirrorID);  
                        playerMirrorNI.SetSocketReference(this);

                        if(ni.IsLocalPlayer)
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
                            CircleCollider2D coll = spawnedObject.GetComponent<CircleCollider2D>();
                            coll.isTrigger = true;
                        }

                        //Sets the name of the Player to the ID
                        spawnedObject.name = string.Format("Player ({0})", id);
                        spawnedObject2.name = string.Format("Player ({0})", mirrorID);

                        serverObjects.Add(mirrorID, playerMirrorNI);
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
    public class SpawnData
    {
        public string id;
        public string name;
        public Vector2Data position;
        public float rotation;
        public bool spawnMirror;
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

    // This Serializable Class is used for messages that involve Taking and Leaving Seats
    [Serializable]
    public class SeatMove
    {
        public Vector2Data position;
        public float rotation;
    }

    [Serializable]
    public class Damage
    {
        public string senderID;
        public string receiverID;
        public float damage;
    }
}
