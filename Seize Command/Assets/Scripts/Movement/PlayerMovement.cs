using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    public class PlayerMovement : AbstractMovement
    {
        protected override void Move()
        {
            transform.position += new Vector3(x, y, 0) * speed * Time.deltaTime;
        }
    }
}