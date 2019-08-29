using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Aiming
{
    public abstract class AbstractAim : MonoBehaviour
    {
        protected const float BARREL_PIVOT_OFFSET = 90.0f;

        protected virtual void Update()
        {
            Aim();
        }

        protected abstract void Aim();
    }
}