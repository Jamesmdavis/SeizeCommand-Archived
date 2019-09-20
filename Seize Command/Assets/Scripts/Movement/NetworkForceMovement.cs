using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;
using SeizeCommand.Utility;

namespace SeizeCommand.Movement
{
    //Multiplayer Version of ForceMovement

    //This script is not Hack proof AT ALL
    //The reason for this is because of rb.addforce.  I don't know how it works so I can't
    //call it on the Javascript Server to verify correct positions
    public class NetworkForceMovement : AbstractMovement
    {
        [SerializeField] private float thrust;

        protected NetworkIdentity networkIdentity;

        protected override void Start()
        {
            base.Start();
            networkIdentity = GetComponent<NetworkIdentity>();
        }

        //The Ship can only move on the y plane because of the thrusters in the back
        //-transform.up represents the direction the ship is moving
        //The ship moves in the direction it is facing with a speed
        protected override void Move()
        {
            base.Move();

            SendInputData();
            /*
            Vector2 forceDirection = y * transform.up * thrust;
            rb.AddForce(forceDirection);
            */
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            /*
            if(rb.velocity != Vector2.zero)
            {
                SendData();
            }
            */
        }

        private void SendInputData()
        {
            ForceMove package = new ForceMove();
            package.velocity = new Vector2Data();
            package.velocity.x = rb.velocity.x;
            package.velocity.y = rb.velocity.y;
            package.thrust = thrust;
            package.deltaTime = Time.deltaTime;

            NetworkIdentity ni = controller.GetComponent<NetworkIdentity>();
            ni.Socket.Emit("forceMove", new JSONObject(JsonUtility.ToJson(package)));

            /*
            Vector2Data package = new Vector2Data();
            package.x = transform.position.x;
            package.y = transform.position.y;

            networkIdentity.Socket.Emit("shipMove", new JSONObject(JsonUtility.ToJson(package)));
            */
        }
    }
}