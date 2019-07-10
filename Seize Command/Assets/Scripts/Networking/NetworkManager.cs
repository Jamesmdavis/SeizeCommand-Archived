using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        [Serializable]
        public class MovementSendPackage
        {
            public float horizontal;
            public float vertical;
            public float timeStamp;
        }

        [Serializable]
        public class MovementReceivePackage
        {
            public float x;
            public float y;
            public float z;
            public float timeStamp;
        }
    }
}
