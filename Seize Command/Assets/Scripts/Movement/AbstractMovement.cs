using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    public abstract class AbstractMovement : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] protected float speed;
        [SerializeField] private bool horizontal;
        [SerializeField] private bool vertical;

        protected Rigidbody2D rb;
        protected bool isMoving;
        protected float x;
        protected float y;

        public bool IsMoving
        {
            get { return isMoving; }
            set { isMoving = value; }
        }

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
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
