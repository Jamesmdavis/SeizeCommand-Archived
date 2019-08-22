using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Movement_Tim : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float thrustModifier = 1;
    [SerializeField] private float maxSpeed = 10;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        bool Fwd = Input.GetKey("t");
        bool Rvs = Input.GetKey("r");
        float velocity = rb.velocity.magnitude;

        ThrustForward(Fwd);
        ThrustReverse(Rvs);
        VelocityCap(velocity);
    }

    private void VelocityCap(float speed)
    {
        if (speed >= maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    //TrustForward method will propel the craft in the up direction of it's axis when the "t" key is pressed
    private void ThrustForward(bool forward)
    {
        if(forward  == true)
        {
            Vector2 force = transform.up * thrustModifier;
            rb.AddForce(force);
        }
    }

    //ThrustReverse method will create a force in the reverse direction when the "r" key is pressed
    private void ThrustReverse(bool reverse)
    {
        if( reverse == true)
        {
            Vector2 force = -transform.up * thrustModifier;
            rb.AddForce(force);
        }
    }
}
