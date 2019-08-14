using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    public class ShipMovement : AbstractMovement
    {
        protected override void Move()
        {
            Vector2 forceDirection = new Vector2(x * speed, y * speed);
            rb.AddForce(forceDirection);
        }
    }
}