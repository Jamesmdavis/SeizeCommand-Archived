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
        protected Vector3 inputs;

        public virtual InputManager Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            controller = GetComponent<InputManager>();
            inputs = new Vector3();
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
            float xInput = horizontal ? Input.GetAxisRaw("Horizontal") : 0;
            float yInput = vertical ?   Input.GetAxisRaw("Vertical") : 0;

            inputs.x = xInput;
            inputs.y = yInput;
        }

        public virtual void CheckInput()
        {
            isMoving = Input.GetAxisRaw("Horizontal") != 0
                || Input.GetAxisRaw("Vertical") != 0 ? true : false;
        }
    }
}
