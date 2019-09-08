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
            set { movementScript = value; }
        }

        public AbstractAim AimScript
        {
            get { return aimScript; }
            set { aimScript = value; }
        }

        public AttackManager AttackScript
        {
            get { return attackScript; }
            set { attackScript = value; }
        }

        private void Start()
        {
            movementScript = GetComponent<AbstractMovement>();
            aimScript = GetComponent<AbstractAim>();
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
            movementScript.IsMoving = Input.GetAxis("Horizontal") != 0
                || Input.GetAxis("Vertical") != 0 ? true : false;
        }

        private void CheckAimInput()
        {

        }

        private void CheckAttackInput()
        {
            if(Input.GetMouseButton(0))
            {
                attackScript.Attack();
            }
        }
    }
}