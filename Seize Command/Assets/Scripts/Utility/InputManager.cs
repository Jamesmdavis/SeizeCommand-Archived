using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Movement;
using SeizeCommand.Aiming;
using SeizeCommand.Attack;
using SeizeCommand.Networking;

namespace SeizeCommand.Utility
{
    public class InputManager : MonoBehaviour
    {
        private AbstractMovement movementScript;
        private AbstractAim aimScript;
        private AttackManager attackScript;
        private NetworkIdentity networkIdentity;

        public AbstractMovement MovementScript
        {
            get { return movementScript; }
            set
            {
                movementScript = value;
                movementScript.Controller = this;
            }
        }

        public AbstractAim AimScript
        {
            get { return aimScript; }
            set
            {
                aimScript = value;
                aimScript.Controller = this;
            }
        }

        public AttackManager AttackScript
        {
            get { return attackScript; }
            set
            {
                attackScript = value;
                attackScript.Controller = this;
            }
        }

        private void Start()
        {
            movementScript = GetComponent<AbstractMovement>();
            attackScript = GetComponent<AttackManager>();
            networkIdentity = GetComponent<NetworkIdentity>();
        }

        private void Update()
        {
            if(networkIdentity)
            {
                if(networkIdentity.IsLocalPlayer)
                {
                    CheckMovementInput();
                    CheckAimInput();
                    CheckAttackInput();
                }
            }
        }

        private void CheckMovementInput()
        {
            movementScript.IsMoving = Input.GetAxisRaw("Horizontal") != 0
                || Input.GetAxisRaw("Vertical") != 0 ? true : false;
        }

        private void CheckAimInput()
        {
            if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            {
                aimScript.Aim();
            }
        }

        private void CheckAttackInput()
        {
            if(Input.GetMouseButton(0)) attackScript.Attack();
        }
    }
}