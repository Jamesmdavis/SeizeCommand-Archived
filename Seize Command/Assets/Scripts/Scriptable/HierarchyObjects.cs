using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Scriptable
{
    [CreateAssetMenu(fileName = "Hierarchy_Objects", menuName = "Scriptable Objects/Hierarchy Objects", 
        order = 3)]
    public class HierarchyObjects : ScriptableObject
    {
        public List<HierarchyObjectData> Objects;

        public HierarchyObjectData GetObjectByName(string name)
        {
            return Objects.FirstOrDefault(x => x.name == name);
        }
    }

    [Serializable]
    public class HierarchyObjectData
    {
        public string name;
        public Transform location;
    }
}