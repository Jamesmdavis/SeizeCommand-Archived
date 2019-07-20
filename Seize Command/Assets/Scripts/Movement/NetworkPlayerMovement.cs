using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Movement
{
    public class NetworkPlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;

        [Header("Networking Data")]
        [SerializeField] private bool clientPrediction;
        [SerializeField] private float correctionThreshold;

        private NetworkIdentity networkIdentity;
        private Dictionary<float, Vector3> predictedPositions;
        private bool isMoving;


        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            predictedPositions = new Dictionary<float, Vector3>();
            isMoving = false;
        }

        private void FixedUpdate()
        {
            if(isMoving)
            {
                float timeSent = Time.time;

                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                SendData(horizontal, vertical, timeSent);

                if(clientPrediction)
                {
                    transform.position += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
                    predictedPositions.Add(timeSent, transform.position);
                }
            }
        }

        private void Update()
        {
            if(networkIdentity)
            {
                if(networkIdentity.IsLocalPlayer)
                {
                    CheckInput();
                }
            }
        }

        private void CheckInput()
        {
            isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0
                ? true : false;
        }

        private void SendData(float horizontal, float vertical, float timeSent)
        {
            UpdatePosition updatePosition = new UpdatePosition();
            updatePosition.horizontal = horizontal;
            updatePosition.vertical = vertical;
            updatePosition.speed = speed;
            updatePosition.deltaTime = Time.deltaTime;
            updatePosition.timeSent = timeSent;

            networkIdentity.Socket.Emit("updatePosition", new JSONObject(JsonUtility.ToJson(updatePosition)));
        }

        protected void Move()
        {

        }

        public void CorrectPosition(float timeSent, Vector3 serverPosition)
        {
            if(clientPrediction && networkIdentity.IsLocalPlayer)
            {
                Vector3 predictedPosition = predictedPositions[timeSent];

                float dist = Vector3.Distance(transform.position, predictedPosition);

                if(dist >= correctionThreshold)
                {
                    transform.position = serverPosition;
                }

                //Remove old predictedPositions from the Dictionary
            }
            else
            {
                transform.position = serverPosition;
            }
        }
    }
}