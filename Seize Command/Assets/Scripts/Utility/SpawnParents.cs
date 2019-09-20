using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Utility
{
    public class SpawnParents : MonoBehaviour
    {
        [SerializeField] private List<SpawnParentData> Objects;

        public Transform GetParentByName(string name)
        {
            return Objects.FirstOrDefault(x => x.Name == name).Parent;
        }
    }

    [Serializable]
    public class SpawnParentData
    {
        [SerializeField] private string name;
        [SerializeField] private Transform parent;

        public string Name
        {
            get { return name; }
        }

        public Transform Parent
        {
            get { return parent; }
        }
    } 
}