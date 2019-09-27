using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Weapon
{
    public class NetworkGun : AbstractGun
    {
        private NetworkIdentity networkIdentity;
        private SpawnPackage package;

        private void Start()
        {
            networkIdentity = GetComponentInParent<NetworkIdentity>();
            package = new SpawnPackage();
            package.position = new Vector2Data();
        }

        public override void Fire()
        {
            SendData();
        }

        private void SendData()
        {
            package.name = projectile.name;
            package.position.x = projectileSpawnPoint.position.x;
            package.position.y = projectileSpawnPoint.position.y;
            package.rotation = projectileSpawnPoint.rotation.z;

            networkIdentity.Socket.Emit("serverSpawnMirroredPair", 
                new JSONObject(JsonUtility.ToJson(package)));
        }
    }
}