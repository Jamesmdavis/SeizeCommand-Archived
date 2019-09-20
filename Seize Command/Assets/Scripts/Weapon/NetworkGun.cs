using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Networking;

namespace SeizeCommand.Weapon
{
    public class NetworkGun : AbstractGun
    {
        private NetworkIdentity networkIdentity;
        private SpawnData bulletData;

        private void Start()
        {
            networkIdentity = GetComponentInParent<NetworkIdentity>();
            bulletData = new SpawnData();
            bulletData.position = new Vector2Data();
            bulletData.parent = new Vector2Data();
        }

        public override void Fire()
        {
            bulletData.name = projectile.name;
            bulletData.position.x = projectileSpawnPoint.position.x;
            bulletData.position.y = projectileSpawnPoint.position.y;
            bulletData.rotation = projectileSpawnPoint.rotation.z;
            bulletData.parent.x = projectileParent.position.x;
            bulletData.parent.y = projectileParent.position.y;

            networkIdentity.Socket.Emit("serverSpawn", new JSONObject(JsonUtility.ToJson(bulletData)));
        }
    }
}