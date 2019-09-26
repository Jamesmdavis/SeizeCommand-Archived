using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Movement
{
    public class SetVelocity : MonoBehaviour
    {
        [SerializeField] private float speed;

        private Rigidbody2D rb;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            Move();
        }

        private void Move()
        {
            rb.velocity = transform.up * speed;
        }
    }
}