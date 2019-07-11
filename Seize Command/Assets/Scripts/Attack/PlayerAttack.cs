using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Attack
{
    public class PlayerAttack : AbstractNetworkAttack
    {
        protected override void CheckAttack()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }
    }
}