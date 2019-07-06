using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : AbstractMovement
{
    protected override void Move()
    {
        rb.MovePosition(transform.position + transform.up * strength * Time.deltaTime);
    }
}