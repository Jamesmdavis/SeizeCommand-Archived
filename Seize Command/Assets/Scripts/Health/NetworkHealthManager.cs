using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Health
{
    public class NetworkHealthManager : HealthManager
    {
        private NetworkIdentity networkIdentity;
        private Damage damageMessage;

        protected override void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            damageMessage = new Damage();
        }

        public override void TakeDamage(GameObject sender, float damage)
        {
            SendData(sender, damage);
        }

        protected override void Die()
        {
            if(networkIdentity.IsLocalPlayer)
            {
                networkIdentity.Socket.Emit("respawn");
            }
            
            base.Die();
        }

        public void InduceDamage(float damage)
        {
            ApplyDamage(damage);
        }

        private void SendData(GameObject sender, float damage)
        {
            NetworkIdentity senderNetworkIdentity = sender.GetComponent<NetworkIdentity>();
            string senderID = senderNetworkIdentity.ID;
            string receiverID = networkIdentity.ID;

            damageMessage.senderID = senderID;
            damageMessage.receiverID = receiverID;
            damageMessage.damage = damage;

            networkIdentity.Socket.Emit("takeDamage", new JSONObject(JsonUtility.ToJson(damageMessage)));
        }
    }
}