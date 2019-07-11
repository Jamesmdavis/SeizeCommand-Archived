using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Attack
{
    public class PlayerAttack : AbstractAttack
    {
        protected override void Attack()
        {
            if(Input.GetMouseButtonDown(0))
            {
                gun.Fire();
            }
        }
    }
}