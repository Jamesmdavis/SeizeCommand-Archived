using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    public class PlayerMovement : AbstractMovement
    {
        protected override void Move()
        {
            //transform.Translate(transform.up * strength * Time.deltaTime);
            //transform.position += transform.up * strength * Time.deltaTime;
            rb.MovePosition(transform.position + transform.up * strength * Time.deltaTime);
        }
    }
}