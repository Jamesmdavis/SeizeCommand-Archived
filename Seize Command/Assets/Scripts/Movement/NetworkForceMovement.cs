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

        private NetworkIdentity networkIdentity;

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
            Vector2 forceDirection = inputs.y * transform.up * thrust;
            rb.AddForce(forceDirection);
            SendData();
        }

        private void SendData()
        {
            Vector2Package package = new Vector2Package();
            package.id = networkIdentity.ID;
            package.vector2 = new Vector2Data();
            package.vector2.x = rb.velocity.x;
            package.vector2.y = rb.velocity.y;

            networkIdentity.Socket.Emit("changeVelocity", new JSONObject(JsonUtility.ToJson(package)));
        }
    }
}