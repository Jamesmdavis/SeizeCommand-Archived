using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Movement;
using SeizeCommand.Aim;
using SeizeCommand.Attack;

namespace SeizeCommand.Interactions.Interactables
{
    public abstract class AbstractSeat : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform leaveSeatPosition;

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

            Collider2D playerColl = interactor.Player.GetComponent<Collider2D>();
            Collider2D seatColl = GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(playerColl, seatColl);

            AbstractMovement playerMovement = interactor.Player.GetComponent<AbstractMovement>();
            playerMovement.enabled = false;

            AbstractAim playerAim = interactor.Player.GetComponent<AbstractAim>();
            playerAim.enabled = false;

            AbstractAttack playerAttack = interactor.Player.GetComponent<AbstractAttack>();
            playerAttack.enabled = false;

            interactor.transform.position = transform.position;
            interactor.transform.eulerAngles = new Vector3(0, 0, 0);

            Debug.Log("Take Seat");
        }

        protected virtual void LeaveSeat(Interactor interactor)
        {
            Debug.Log("Leave Seat");
            currentInteractor = null;

            interactor.transform.position = leaveSeatPosition.position;

            Collider2D playerColl = interactor.Player.GetComponent<Collider2D>();
            Collider2D seatColl = GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(playerColl, seatColl, false);

            AbstractMovement playerMovement = interactor.Player.GetComponent<AbstractMovement>();
            playerMovement.enabled = true;

            AbstractAim playerAim = interactor.Player.GetComponent<AbstractAim>();
            playerAim.enabled = true;

            AbstractAttack playerAttack = interactor.Player.GetComponent<AbstractAttack>();
            playerAttack.enabled = true;
        }
    }
}