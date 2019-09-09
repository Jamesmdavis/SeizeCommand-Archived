using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    //This Movement script uses a rigidbody and Forces to move the GameObject
    public class ForceMovement : AbstractMovement
    {
        [SerializeField] private float thrust;

        protected override void Move()
        {
            base.Move();
            
            //The Ship can only move on the y plane because of the thrusters in the back
            //-transform.up represents the direction the ship is moving
            //The ship moves in the direction it is facing with a speed
            Vector2 forceDirection = y * transform.up * thrust;
            rb.AddForce(forceDirection);
        }
    }
}