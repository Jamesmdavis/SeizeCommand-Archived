using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    public abstract class AbstractMovement : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] protected float strength;

        protected Rigidbody2D rb;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void FixedUpdate()
        {
            Move();
        }

        protected abstract void Move();
    }
}
