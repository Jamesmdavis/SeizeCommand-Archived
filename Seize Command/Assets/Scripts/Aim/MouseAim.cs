using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Aiming
{
    //This Aiming script snaps the rotation to that of the mouse position
    public class MouseAim : AbstractAim
    {   
        protected override void Aim()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

            float currentRotation = rot + barrelOffset;

            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }

        public override void CheckInput()
        {
            if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            {
                Aim();
            }
        }
    }
}
