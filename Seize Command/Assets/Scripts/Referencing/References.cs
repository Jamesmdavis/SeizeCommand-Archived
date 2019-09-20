using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Referencing
{
    public class References<T> : MonoBehaviour
    { 
        [SerializeField] private List<ReferenceData<T>> Objects;

        public event Action<ReferenceData<T>> OnReferenceChange;

        public void AddReference(string name, T reference)
        {
            ReferenceData<T> referenceData = new ReferenceData<T>(name, reference);
            Objects.Add(referenceData);
            OnReferenceChange(referenceData);
        }

        public void RemoveReference(string name)
        {
            ReferenceData<T> referenceData = Objects.FirstOrDefault(x => x.Name == name);
            Objects.Remove(referenceData);
            OnReferenceChange(referenceData);
        }

        public void ChangeNameOfReference(string name, string newName)
        {
            ReferenceData<T> referenceData = Objects.FirstOrDefault(x => x.Name == name);
            referenceData.Name = newName;
            OnReferenceChange(referenceData);
        }

        public void ChangeReferenceOfReference(string name, T reference)
        {
            ReferenceData<T> referenceData = Objects.FirstOrDefault(x => x.Name == name);
            referenceData.Reference = reference;
            OnReferenceChange(referenceData);
        }

        public T GetReferenceByName(string name)
        {
            return Objects.FirstOrDefault(x => x.Name == name).Reference;
        }

    }

    [Serializable]
    public class ReferenceData<T>
    {
        [SerializeField] private string name;
        [SerializeField] private T reference;

        public ReferenceData(string N, T R)
        {
            name = N;
            reference = R;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public T Reference
        {
            get { return reference; }
            set { reference = value; }
        }
    }
}