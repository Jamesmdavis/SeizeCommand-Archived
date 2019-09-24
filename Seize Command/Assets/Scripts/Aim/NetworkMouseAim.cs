using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;
using SeizeCommand.Utility;

namespace SeizeCommand.Aiming
{
    //Multiplayer version of MouseAim
    public class NetworkMouseAim : AbstractAim
    {   
        private NetworkIdentity networkIdentity;

        protected override void Start()
        {
            base.Start();
            networkIdentity = GetComponent<NetworkIdentity>();
        }

        protected override void Aim()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

            float currentRotation = rot + barrelOffset;

            SendData(currentRotation);
        }

        private void SendData(float currentRotation)
        {
            RotationPackage package = new RotationPackage();
            package.id = networkIdentity.ID;
            package.rotation = currentRotation;

            networkIdentity.Socket.Emit("changeRotation", new JSONObject(JsonUtility.ToJson(package)));
        }

        public override void CheckInput()
        {
            if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            {
                Aim();
            }
        }
    }

}