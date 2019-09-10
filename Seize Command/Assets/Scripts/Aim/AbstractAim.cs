using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Utility;

namespace SeizeCommand.Aiming
{
    public abstract class AbstractAim : MonoBehaviour
    {
        [SerializeField] protected float barrelOffset;

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

        protected abstract void Aim();

        public virtual void CheckInput()
        {
            Aim();
        }
    }
}