using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Aiming
{
    //This Aiming script snaps the rotation to that of the mouse position
    public class MouseAim : AbstractAim
    {   
        public override void Aim()
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
