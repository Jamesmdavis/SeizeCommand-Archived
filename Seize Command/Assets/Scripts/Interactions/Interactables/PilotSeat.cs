using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactors;

namespace SeizeCommand.Interactions.Interactables
{
    public class PilotSeat : AbstractSeat
    {
        protected override void TakeSeat(Interactor interactor)
        {
            base.TakeSeat(interactor);
        }

        protected override void LeaveSeat(Interactor interactor)
        {
            base.LeaveSeat(interactor);
        }
    }
}