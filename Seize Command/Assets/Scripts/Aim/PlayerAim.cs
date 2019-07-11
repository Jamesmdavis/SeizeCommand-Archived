﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Aim
{
    public class PlayerAim : AbstractAim
    {
        protected override void Aim()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
    }
}
