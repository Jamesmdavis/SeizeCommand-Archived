using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    public class ShipProjectileMovement : AbstractMovement
    {
        [SerializeField] private float speed;
        [SerializeField] private float direction;

        private Rigidbody2D shipRb;
        
        protected override void Start()
        {
            base.Start();
            shipRb = GetComponentInParent<Rigidbody2D>();
            Move();
        }

        protected override void FixedUpdate() {}

        protected override void Move()
        {
            rb.velocity = shipRb.velocity + (Vector2)transform.up * direction * speed;
        }
    }
}