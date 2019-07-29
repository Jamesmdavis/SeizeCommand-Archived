using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Movement;
using SeizeCommand.Aim;

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

            Debug.Log("Take Seat");

            AbstractMovement playerMovement = interactor.Player.GetComponent<AbstractMovement>();
            playerMovement.enabled = false;

            AbstractAim playerAim = interactor.Player.GetComponent<AbstractAim>();
            playerAim.enabled = false;

            Transform playerTransform = interactor.transform;
            playerTransform.position = transform.position;
            playerTransform.eulerAngles = new Vector3(0, 0, 0);
        }

        protected virtual void LeaveSeat(Interactor interactor)
        {
            Debug.Log("Leave Seat");
            currentInteractor = null;

            Transform playerTransform = interactor.transform;
            playerTransform.position = leaveSeatPosition.position;

            Collider2D[] playerColls = interactor.Player.GetComponentsInChildren<Collider2D>();
            Collider2D seatColl = GetComponent<Collider2D>();

            for(int i = 0; i < playerColls.Length; i++)
            {
                Physics2D.IgnoreCollision(playerColls[i], seatColl, false);
            }

            AbstractMovement playerMovement = interactor.Player.GetComponent<AbstractMovement>();
            playerMovement.enabled = true;

            AbstractAim playerAim = interactor.Player.GetComponent<AbstractAim>();
            playerAim.enabled = true;
        }
    }
}