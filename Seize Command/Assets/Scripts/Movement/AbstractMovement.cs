using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Utility;

namespace SeizeCommand.Movement
{
    public abstract class AbstractMovement : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private bool horizontal;
        [SerializeField] private bool vertical;

        protected Rigidbody2D rb;
        protected InputManager controller;
        protected bool isMoving;
        protected float x;
        protected float y;

        public virtual InputManager Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        public bool IsMoving
        {
            get { return isMoving; }
            set { isMoving = value; }
        }

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            controller = GetComponent<InputManager>();
        }

        protected virtual void FixedUpdate()
        {
            if(isMoving)
            {
                Move();
            }
        }

        protected virtual void Move()
        {
            if(horizontal) x = Input.GetAxisRaw("Horizontal");
            if(vertical)   y = Input.GetAxisRaw("Vertical");
        }
    }
}
