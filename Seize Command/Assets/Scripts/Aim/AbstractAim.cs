using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Utility;

namespace SeizeCommand.Aiming
{
    public abstract class AbstractAim : MonoBehaviour
    {
        protected const float BARREL_PIVOT_OFFSET = 90.0f;

        protected InputManager controller;

        public virtual InputManager Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        protected virtual void Start()
        {
            controller = GetComponent<InputManager>();
        }

        public abstract void Aim();
    }
}