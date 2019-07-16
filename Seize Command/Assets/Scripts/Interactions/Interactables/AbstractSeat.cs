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
        [SerializeField] private Vector3 leaveSeatPosition;

        private PlayerInteractor currentInteractor;

        public void Interact(PlayerInteractor interactor)
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

        protected virtual void TakeSeat(PlayerInteractor interactor)
        {
            currentInteractor = interactor;

            Collider2D[] playerColls = interactor.Player.GetComponentsInChildren<Collider2D>();
            Collider2D seatColl = GetComponent<Collider2D>();

            for(int i = 0; i < playerColls.Length; i++)
            {
                Physics2D.IgnoreCollision(playerColls[i], seatColl);
            }

            AbstractMovement playerMovement = interactor.GetComponent<AbstractMovement>();
            playerMovement.enabled = false;

            AbstractAim playerAim = interactor.GetComponent<AbstractAim>();
            playerAim.enabled = false;

            Transform playerTransform = interactor.transform;
            playerTransform.position = transform.position;
            playerTransform.eulerAngles = new Vector3(0, 0, 0);
        }

        protected virtual void LeaveSeat(PlayerInteractor interactor)
        {
            currentInteractor = null;

            Transform playerTransform = interactor.transform;
            playerTransform.position = leaveSeatPosition;

            Collider2D[] playerColls = interactor.Player.GetComponentsInChildren<Collider2D>();
            Collider2D seatColl = GetComponent<Collider2D>();

            for(int i = 0; i < playerColls.Length; i++)
            {
                Physics2D.IgnoreCollision(playerColls[i], seatColl, false);
            }

            AbstractMovement playerMovement = interactor.GetComponent<AbstractMovement>();
            playerMovement.enabled = true;

            AbstractAim playerAim = interactor.GetComponent<AbstractAim>();
            playerAim.enabled = true;
        }
    }
}