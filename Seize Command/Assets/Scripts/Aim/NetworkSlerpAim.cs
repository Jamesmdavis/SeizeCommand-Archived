using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;
using SeizeCommand.Interactions.Interactables;

namespace SeizeCommand.Aiming
{
    //Multiplayer version of SlerpAim
    public class NetworkSlerpAim : AbstractAim
    {
        [Header("Data")]
        [SerializeField] [Range(1f, 10f)] private float intensity;

        private NetworkIdentity networkIdentity;
        private Aim aimMessage;
        private float oldRotation;

        public NetworkIdentity NetworkIdentity
        {
            get { return networkIdentity; }
            set { networkIdentity = value; }
        }

        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            aimMessage = new Aim();
            oldRotation = 0f;
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

        private void SendData(float rot)
        {
            aimMessage.rotation = rot;
            networkIdentity.Socket.Emit("shipAim", new JSONObject(JsonUtility.ToJson(aimMessage)));
        }

        private void CheckForChangeInRotation(float currentRotation)
        {
            if(currentRotation != oldRotation)
            {
                transform.rotation = Quaternion.Euler(0, 0, currentRotation);
                SendData(currentRotation);
            }

            oldRotation = currentRotation;
        }

        protected override void Aim()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

            float endRotationFloat = rot + BARREL_PIVOT_OFFSET + 180f;

            Quaternion currentRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(0, 0, endRotationFloat);

            Quaternion newRotation = Quaternion.Slerp(currentRotation, endRotation,
                intensity * Time.deltaTime);

            float newZRotation = newRotation.eulerAngles.z;
            CheckForChangeInRotation(newZRotation);
        }
    }
}