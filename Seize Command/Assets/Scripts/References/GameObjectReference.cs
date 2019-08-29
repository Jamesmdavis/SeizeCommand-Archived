using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.References
{
    public class GameObjectReference : MonoBehaviour
    {
        [SerializeField] private GameObject reference;
        
        public GameObject Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                if(OnReferenceChange != null) OnReferenceChange(reference);
            }
        }

        public event Action<GameObject> OnReferenceChange;
    }
}