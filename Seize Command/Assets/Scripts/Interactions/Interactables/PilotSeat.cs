using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Movement;

using SeizeCommand.Interactions.Interactors;

namespace SeizeCommand.Interactions.Interactables
{
    public class PilotSeat : AbstractNetworkSeat
    {
        protected override void TakeSeat(Interactor interactor)
        {
            base.TakeSeat(interactor);

            AbstractMovement movement = GetComponentInParent<AbstractMovement>();
            movement.enabled = true;
        }

        protected override void LeaveSeat(Interactor interactor)
        {
            base.LeaveSeat(interactor);

            AbstractMovement movement = GetComponentInParent<AbstractMovement>();
            movement.enabled = false;
        }
    }
}