using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Movement_Tim : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float thrustModifier = 1;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        bool Fwd = Input.GetKey("t");
        bool Reverse = Input.GetKey("r");

        ThrustForward(Fwd);
        ThrustReverse(Reverse);
    }

    //TrustForward method will propel the craft in the up direction of it's axis when the "t" key is pressed
    private void ThrustForward(bool Fwd)
    {
        if(Fwd  == true)
        {
            Vector2 force = transform.up * thrustModifier;
            rb.AddForce(force);
        }
    }

    //ThrustReverse method will create a force in the reverse direction when the "r" key is pressed
    private void ThrustReverse(bool Reverse)
    {
        if( Reverse == true)
        {
            Vector2 force = -transform.up * thrustModifier;
            rb.AddForce(force);
        }
    }
}
