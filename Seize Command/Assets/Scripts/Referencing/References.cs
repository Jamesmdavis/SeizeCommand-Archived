using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Referencing
{
    public class References : MonoBehaviour
    { 
        [SerializeField] private List<ReferenceData> Objects;

        public event Action<ReferenceData> OnReferenceChange;

        public void AddReference(string name, GameObject reference)
        {
            ReferenceData referenceData = new ReferenceData(name, reference);
            Objects.Add(referenceData);
            if(OnReferenceChange != null) OnReferenceChange(referenceData);
        }

        public void RemoveReference(string name)
        {
            ReferenceData referenceData = Objects.FirstOrDefault(x => x.Name == name);
            Objects.Remove(referenceData);
        }

        public void ChangeNameOfReference(string name, string newName)
        {
            ReferenceData referenceData = Objects.FirstOrDefault(x => x.Name == name);
            referenceData.Name = newName;
        }

        public void ChangeReferenceOfReference(string name, GameObject reference)
        {
            ReferenceData referenceData = Objects.FirstOrDefault(x => x.Name == name);
            referenceData.Reference = reference;
        }

        public GameObject GetReferenceByName(string name)
        {
            return Objects.FirstOrDefault(x => x.Name == name).Reference;
        }
    }

    [Serializable]
    public class ReferenceData
    {
        [SerializeField] private string name;
        [SerializeField] private GameObject reference;

        public ReferenceData(string N, GameObject R)
        {
            name = N;
            reference = R;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public GameObject Reference
        {
            get { return reference; }
            set { reference = value; }
        }
    }
}