using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Aiming
{
    //This class is empty only temporarily.  Once I add Ship Aiming I will move the current
    //Implementation of Aim to this class and make it abstract in AbstractAim

    public class PlayerAim : AbstractAim
    {
        [Header("Data")]
        [SerializeField] protected float speed;

        protected override void Aim()
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
