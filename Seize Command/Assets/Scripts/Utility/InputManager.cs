using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Movement;
using SeizeCommand.Aiming;
using SeizeCommand.Attack;
using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Networking;

namespace SeizeCommand.Utility
{
    public class InputManager : MonoBehaviour
    {
        private AbstractMovement movementScript;
        private AbstractAim aimScript;
        private AttackManager attackScript;
        [SerializeField] private Interactor interactorScript;
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

        public Interactor InteractorScript
        {
            get { return interactorScript; }
            set
            {
                interactorScript = value;
                interactorScript.Controller = this;
            }
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
                    if(movementScript)      movementScript.CheckInput();
                    if(aimScript)           aimScript.CheckInput();
                    if(attackScript)        attackScript.CheckInput();
                    if(interactorScript)    interactorScript.CheckInput();
                }
            }
        }
    }
}