using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Aim
{
    public abstract class AbstractAim : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] protected float speed;

        public float Rotation
        {
            get;
            protected set;
        }

        protected virtual void Start() { }

        protected virtual void FixedUpdate()
        {
            Aim();
        }

        protected abstract void Aim();
    }
}