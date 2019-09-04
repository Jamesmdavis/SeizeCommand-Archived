using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    public class ProjectileMovement : AbstractMovement
    {
        protected override void Start()
        {
            base.Start();
            Move();
        }

        protected override void FixedUpdate() {}

        protected override void Move()
        {
            rb.velocity = transform.up * speed;
        }
    }
}