using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Scriptable
{
    [CreateAssetMenu(fileName = "Server_Objects", menuName = "Scriptable Objects/Server Objects",
        order = 3)]
    public class ServerObjects : ScriptableObject
    {
        public List<ServerObjectData> Objects;

        public ServerObjectData GetObjectByName(string name)
        {
            return Objects.SingleOrDefault(x => x.name == name);
        }
    }

    [Serializable]
    public class ServerObjectData
    {
        public string name;
        public GameObject prefab;
    }
}