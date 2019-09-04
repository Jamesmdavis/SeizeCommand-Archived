using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Movement
{
    //This script is not Hack proof AT ALL
    //The reason for this is because of rb.addforce.  I don't know how it works so I can't
    //call it on the Javascript Server to verify correct positions
    public class NetworkShipMovement : AbstractMovement
    {
        private NetworkIdentity networkIdentity;

        protected override void Start()
        {  
            base.Start();
            networkIdentity = GetComponent<NetworkIdentity>();
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

        //The Ship can only move on the y plane because of the thrusters in the back
        //-transform.up represents the direction the ship is moving
        //The ship moves in the direction it is facing with a speed
        protected override void Move()
        {
            Vector2 forceDirection = y * transform.up * speed;
            rb.AddForce(forceDirection);
        }

        private void SendData()
        {
            Position package = new Position();
            package.x = transform.position.x;
            package.y = transform.position.y;

            networkIdentity.Socket.Emit("shipMove", new JSONObject(JsonUtility.ToJson(package)));
        }
    }
}