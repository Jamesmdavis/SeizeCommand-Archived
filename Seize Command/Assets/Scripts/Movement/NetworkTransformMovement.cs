using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Movement
{
    public class NetworkTransformMovement : AbstractMovement
    {
        [SerializeField] private float speed;

        private NetworkIdentity networkIdentity;
        
        protected override void Start()
        {
            base.Start();
            networkIdentity = GetComponent<NetworkIdentity>();
        }

        protected override void Move()
        {
            base.Move();
            transform.Translate(inputs * speed * Time.deltaTime);
            SendData();
        }

        private void SendData()
        {
            Vector2Package package = new Vector2Package();
            package.id = networkIdentity.ID;
            package.vector2 = new Vector2Data();
            package.vector2.x = transform.position.x;
            package.vector2.y = transform.position.y;

            networkIdentity.Socket.Emit("changePosition", new JSONObject(JsonUtility.ToJson(package)));
        }
    }
}