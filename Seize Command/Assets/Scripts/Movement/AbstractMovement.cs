using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    public abstract class AbstractMovement : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] protected float speed;

        protected Rigidbody2D rb;
        protected bool isMoving;

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

        protected virtual void Update()
        {
            CheckInput();
        }

        protected abstract void Move();

        private void CheckInput()
        {
            isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0
                ? true : false;
        }
    }
}
