using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Aiming
{
    //This Aiming script uses a Slerp to smoothly rotate towards the mouse position
    public class SlerpAim : AbstractAim
    {
        [Header("Data")]
        [SerializeField] [Range(1f, 10f)] private float intensity;

        protected override void Aim()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

            float endRotationFloat = rot + barrelOffset;

            Quaternion currentRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(0, 0, endRotationFloat);

            Quaternion newRotation = Quaternion.Slerp(currentRotation, endRotation,
                intensity * Time.deltaTime);

            transform.rotation = newRotation;
        }
    }
}