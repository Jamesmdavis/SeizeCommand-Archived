using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Utility
{
    public class PlayerReference : MonoBehaviour
    {
        public GameObject Reference
        {
            get;
            private set;
        }

        public void SetReference(GameObject reference)
        {
            Reference = reference;
        }
    }
}