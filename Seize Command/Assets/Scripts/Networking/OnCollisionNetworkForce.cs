using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Networking
{
    public class OnCollisionNetworkForce : MonoBehaviour
    {
        private NetworkIdentity networkIdentity;
        private Rigidbody2D rb;

        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            SendData();
        }

        private void OnCollisionExit2D(Collision2D coll)
        {
            SendData();
        }

        private void SendData()
        {
            Velocity2D package = new Velocity2D();
            package.id = networkIdentity.ID;
            package.velocity = new Vector2Data();
            package.velocity.x = rb.velocity.x;
            package.velocity.y = rb.velocity.y;

            networkIdentity.Socket.Emit("updateVelocity", new JSONObject(JsonUtility.ToJson(package)));
        }
    }
}