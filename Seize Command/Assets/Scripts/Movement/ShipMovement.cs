using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    public class ShipMovement : AbstractMovement
    {
        protected override void Move()
        {
            //The Ship can only move on the y plane because of the thrusters in the back
            //-transform.up represents the direction the ship is moving
            //The ship moves in the direction it is facing with a speed
            Vector2 forceDirection = y * transform.up * speed;
            rb.AddForce(forceDirection);
        }
    }
}