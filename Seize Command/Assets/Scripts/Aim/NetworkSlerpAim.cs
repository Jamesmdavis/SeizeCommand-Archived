using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;
using SeizeCommand.Utility;

namespace SeizeCommand.Aiming
{
    //Multiplayer version of SlerpAim
    public class NetworkSlerpAim : AbstractAim
    {
        [Header("Data")]
        [SerializeField] [Range(1f, 10f)] private float intensity;

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

            float endRotationFloat = rot + barrelOffset;

            Quaternion currentRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(0, 0, endRotationFloat);

            Quaternion newRotation = Quaternion.Slerp(currentRotation, endRotation,
                intensity * Time.deltaTime);

            float newZRotation = newRotation.eulerAngles.z;

            SendData(newZRotation);
        }

        private void SendData(float currentRotation)
        {
            RotationPackage package = new RotationPackage();
            package.id = networkIdentity.ID;
            package.rotation = currentRotation;
            
            networkIdentity.Socket.Emit("changeRotation", new JSONObject(JsonUtility.ToJson(package)));
        }
    }
}