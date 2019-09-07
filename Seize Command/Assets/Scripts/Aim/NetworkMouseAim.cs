using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Aiming
{
    //Multiplayer version of MouseAim
    public class NetworkMouseAim : AbstractAim
    {   
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

        private void SendData(float rot)
        {
            aimMessage.rotation = rot;
            networkIdentity.Socket.Emit("aim", new JSONObject(JsonUtility.ToJson(aimMessage)));
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
    
        protected override void Update()
        {
            if(networkIdentity)
            {
                if(networkIdentity.IsLocalPlayer)
                {
                    Aim();
                }
            }
        }

        protected override void Aim()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

            float currentRotation = rot + BARREL_PIVOT_OFFSET;
            
            CheckForChangeInRotation(currentRotation);
        }
    }

}