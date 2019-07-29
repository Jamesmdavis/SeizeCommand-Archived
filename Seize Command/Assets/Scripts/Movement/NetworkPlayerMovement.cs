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

        private NetworkIdentity networkIdentity;
        private Dictionary<float, Vector3> predictedPositions;

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

        protected override void Move()
        {
            float timeSent = Time.time;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            SendData(horizontal, vertical, timeSent);

            if(clientPrediction)
            {
                transform.position += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
                predictedPositions.Add(timeSent, transform.position);
            }
        }

        private void SendData(float horizontal, float vertical, float timeSent)
        {
            UpdatePosition updatePosition = new UpdatePosition();
            updatePosition.horizontal = horizontal;
            updatePosition.vertical = vertical;
            updatePosition.speed = speed;
            updatePosition.deltaTime = Time.deltaTime;
            updatePosition.timeSent = timeSent;

            updatePosition.horizontal = Mathf.Round(updatePosition.horizontal * 100f) / 100f;
            updatePosition.vertical = Mathf.Round(updatePosition.vertical * 100f) / 100f;
            
            networkIdentity.Socket.Emit("updatePosition", new JSONObject(JsonUtility.ToJson(updatePosition)));
        }

        public void CorrectPosition(float timeSent, Vector3 serverPosition)
        {
            if(clientPrediction && networkIdentity.IsLocalPlayer)
            {
                //Vector3 predictedPosition = predictedPositions[timeSent];

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
            else
            {
                transform.position = serverPosition;
            }
        }
    }
}