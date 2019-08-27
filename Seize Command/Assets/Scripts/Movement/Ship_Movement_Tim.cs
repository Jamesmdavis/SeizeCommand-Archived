using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Movement_Tim : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float thrustModifier = 1;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float brakeStrength = 6;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        float dir = Input.GetAxis("Vertical");
        float velocity = rb.velocity.magnitude;
        bool brk = Input.GetKey("b");

        VelocityCap(velocity);
        Thrust(dir);
        Brake(brk);
        
    }

    //This sets a maximum speed for the craft.
    private void VelocityCap(float speed)
    {
        if (speed >= maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    //Thrust will propel the craft foward or reverse depending on the "dir" input direction.  Up on the direction keys will thrust the craft while down will reverse.
    private void Thrust(float direction)
    {
        Vector2 force = transform.up * thrustModifier * direction;
        rb.AddForce(force);
    }

    //This will reduce the speed of the craft.  press the "b" key to do so.  Make the brake modifier value at 1000 or above.  The higher tha modifier, the stronger your brakes
    private void Brake(bool brake)
    {
        if (brake == true)
        {
            rb.velocity = rb.velocity * (1000 / brakeStrength);
        }
    }
}
