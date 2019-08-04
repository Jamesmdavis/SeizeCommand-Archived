using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Aiming
{
    public abstract class AbstractAim : MonoBehaviour
    {
        protected const float BARREL_PIVOT_OFFSET = 90.0f;

        [Header("Data")]
        [SerializeField] protected float speed;

        protected virtual void Update()
        {
            Aim();
        }

        protected virtual void Aim()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

            float currentRotation = rot + BARREL_PIVOT_OFFSET;

            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }
        
    }
}