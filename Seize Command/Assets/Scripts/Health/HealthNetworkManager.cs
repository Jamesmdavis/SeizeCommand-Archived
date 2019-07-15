using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Health
{
    public class HealthNetworkManager : AbstractHealthManager
    {
        private NetworkIdentity myNetworkIdentity;

        private void Start()
        {
            myNetworkIdentity = GetComponent<NetworkIdentity>();
        }

        public override void TakeDamage(GameObject sender, float damage)
        {
            NetworkTakeDamage(sender, damage);
        }

        public void InduceDamage(float damage)
        {
            ApplyDamage(damage);
        }

        public override void Heal(GameObject sender, float heal)
        {
            health += heal;
        }

        private void NetworkTakeDamage(GameObject sender, float damage)
        {
            NetworkIdentity senderNetworkIdentity = sender.GetComponent<NetworkIdentity>();
            string senderID = senderNetworkIdentity.ID;

            TakeDamage takeDamage = new TakeDamage();
            takeDamage.senderID = senderID;
            takeDamage.damage = damage;

            myNetworkIdentity.Socket.Emit("takeDamage", new JSONObject(JsonUtility.ToJson(takeDamage)));
        }
    }
}