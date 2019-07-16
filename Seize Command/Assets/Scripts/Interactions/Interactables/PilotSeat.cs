using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactors;

namespace SeizeCommand.Interactions.Interactables
{
    public class PilotSeat : AbstractSeat
    {
        protected override void TakeSeat(PlayerInteractor interactor)
        {
            base.TakeSeat(interactor);
        }

        protected override void LeaveSeat(PlayerInteractor interactor)
        {
            base.LeaveSeat(interactor);
        }
    }
}