using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMovement : MonoBehaviour
{
    [SerializeField] protected float strength;

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();
}
