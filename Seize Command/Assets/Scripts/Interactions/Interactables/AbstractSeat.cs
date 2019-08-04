using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Movement;
using SeizeCommand.Aiming;
using SeizeCommand.Attack;

namespace SeizeCommand.Interactions.Interactables
{
    public abstract class AbstractSeat : MonoBehaviour, IInteractable
    {
        [SerializeField] protected Transform leaveSeatPosition;

        public Interactor CurrentInteractor
        {
            get { return currentInteractor; }
        }

        private Interactor currentInteractor;

        public void Interact(Interactor interactor)
        {
            if(currentInteractor)
            {
                if(currentInteractor == interactor)
                {
                    LeaveSeat(interactor);
                }
            }
            else
            {
                TakeSeat(interactor);
            }
        }

        protected virtual void TakeSeat(Interactor interactor)
        {
            currentInteractor = interactor;
            interactor.CurrentInteractable = this;

            Collider2D playerColl = interactor.Player.GetComponent<Collider2D>();
            Collider2D seatColl = GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(playerColl, seatColl);

            AbstractMovement playerMovement = interactor.Player.GetComponent<AbstractMovement>();
            playerMovement.enabled = false;

            AbstractAim playerAim = interactor.Player.GetComponent<AbstractAim>();
            playerAim.enabled = false;

            AttackManager playerAttack = interactor.Player.GetComponent<AttackManager>();
            playerAttack.enabled = false;

            Debug.Log("Take Seat");
        }

        protected virtual void LeaveSeat(Interactor interactor)
        {
            currentInteractor = null;
            interactor.CurrentInteractable = null;

            Collider2D playerColl = interactor.Player.GetComponent<Collider2D>();
            Collider2D seatColl = GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(playerColl, seatColl, false);

            AbstractMovement playerMovement = interactor.Player.GetComponent<AbstractMovement>();
            playerMovement.enabled = true;

            AbstractAim playerAim = interactor.Player.GetComponent<AbstractAim>();
            playerAim.enabled = true;

            AttackManager playerAttack = interactor.Player.GetComponent<AttackManager>();
            playerAttack.enabled = true;

            Debug.Log("Leave Seat");
        }
    }
}