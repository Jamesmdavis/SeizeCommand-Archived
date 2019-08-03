using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Movement
{
    public class NetworkPlayerMovement : AbstractMovement
    {
        [Header("Networking Data")]
        [SerializeField] private bool clientPrediction;
        [SerializeField] private float correctionThreshold;
        [SerializeField] private float idleSendDataInterval;

        private NetworkIdentity networkIdentity;
        private Dictionary<float, Vector3> predictedPositions;
        private Coroutine coIdleSendData;
        private bool isColliding;

        protected override void Start()
        {
            base.Start();
            networkIdentity = GetComponent<NetworkIdentity>();
            predictedPositions = new Dictionary<float, Vector3>();
            isMoving = false;
        }

        protected override void Update()
        {
            if(networkIdentity)
            {
                if(networkIdentity.IsLocalPlayer)
                {
                    base.Update();
                }
            }
        }

        protected override void FixedUpdate()
        {
            if(isMoving)
            {
                StopCoroutine(coIdleSendData);
                Move();
            }
            else
            {
                if(coIdleSendData == null)
                {
                    coIdleSendData = StartCoroutine(CoIdleSendData());
                }
            }
        }

        /* 4 Important States Here: Am I Colliding, Am I Using Client Prediction, Both and Neither

        Am I Colliding(isColliding): This check is important because currently the server receives inputs from the clients
        and then creates a new position off of variables such as speed.  However, the server has no way of knowing that a 
        collision is happening so it tries to force the player through it.  IsColliding is meant to tell the server that a collision
        has occured and to give the player temporary authority until the collision has stopped.

        No Client Prediction(!clientPrediction): Works as Follows: Client checks for input, client sends input to server,
        server creates a new position for the player, server sends the new position, client receives and updates the player.

        Client Prediction(clientPrediction): Acts the same way as without client prediction but adds a few more steps to create
        a smoother movement.  The difference is that the client will create a position to move to when the client receives an Input.
        The client will not wait for the server and instead act first.  Finally, when the client receives the correct position from the
        server, it will compare it's position to the server's positon.  If the difference is within the correction threshold, no changes
        is need.  If the difference is greater than the correction threshold, the position of the player is snapped to the server position
         */
        protected override void Move()
        {
            float timeSent = Time.time;

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if(!isColliding)
            {
                SendData(x, y, timeSent);

                if(clientPrediction)
                {
                    float tempSpeed = speed;

                    if(x != 0 && y != 0)
                    {
                        tempSpeed = speed / Mathf.Sqrt(2);
                    }

                    transform.position += new Vector3(x, y, 0) * tempSpeed * Time.deltaTime;
                    predictedPositions.Add(timeSent, transform.position);
                }
            }

            if(isColliding)
            {
                transform.position += new Vector3(x, x, 0) * speed * Time.deltaTime;
                SendCollisionData(x, y);
            }
        }

        // This is a Non-Collision Movement message to the server
        private void SendData(float horizontal, float vertical, float timeSent)
        {
            UpdatePosition package = new UpdatePosition();
            package.horizontal = horizontal;
            package.vertical = vertical;
            package.speed = speed;
            package.deltaTime = Time.deltaTime;
            package.timeSent = timeSent;

            package.horizontal = Mathf.Round(package.horizontal * 100f) / 100f;
            package.vertical = Mathf.Round(package.vertical * 100f) / 100f;
            
            networkIdentity.Socket.Emit("updatePosition", new JSONObject(JsonUtility.ToJson(package)));
        }

        // This is a Collision Movement message to the server
        private void SendCollisionData(float x, float y)
        {   
            CollisionMove package = new CollisionMove();
            package.clientInputs = new Position();
            package.clientPosition = new Position();
            package.clientInputs.x = x;
            package.clientInputs.y = y;
            package.speed = speed;
            package.deltaTime = Time.deltaTime;
            package.clientPosition.x = transform.position.x;
            package.clientPosition.y = transform.position.y;

            package.clientInputs.x = Mathf.Round(package.clientInputs.x * 100f) / 100f;
            package.clientInputs.y = Mathf.Round(package.clientInputs.y * 100f) / 100f;
            package.clientPosition.x = Mathf.Round(package.clientPosition.x * 100f) / 100f;
            package.clientPosition.y = Mathf.Round(package.clientPosition.y * 100f) / 100f;

            networkIdentity.Socket.Emit("collisionMove", new JSONObject(JsonUtility.ToJson(package)));
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            isColliding = true;
        }

        private void OnCollisionExit2D(Collision2D coll)
        {
            isColliding = false;
        }

        public void CorrectPosition(float timeSent, Vector3 serverPosition)
        {
            if(clientPrediction && networkIdentity.IsLocalPlayer)
            {
                if(predictedPositions.ContainsKey(timeSent))
                {
                    float dist = Vector3.Distance(transform.position, serverPosition);

                    if(dist >= correctionThreshold)
                    {
                        transform.position = serverPosition;
                    }

                    foreach(KeyValuePair<float, Vector3> package in predictedPositions)
                    {
                        if(package.Key <= timeSent)
                        {
                            predictedPositions.Remove(package.Key);
                        }
                    }
                }
            }
            else
            {
                transform.position = serverPosition;
            }
        }

        private IEnumerator CoIdleSendData()
        {
            while(true)
            {
                yield return new WaitForSeconds(idleSendDataInterval);

                float x = 0f;
                float y = 0f;
                float timeSent = Time.time;

                if(clientPrediction) predictedPositions.Add(timeSent, transform.position);

                SendData(x, y, timeSent);
            }
        }
    }
}