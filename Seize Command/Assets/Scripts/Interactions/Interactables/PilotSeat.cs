using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Movement;
using SeizeCommand.Aiming;
using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Utility;
using SeizeCommand.References;

namespace SeizeCommand.Interactions.Interactables
{
    public class PilotSeat : AbstractNetworkSeat
    {
        protected override void TakeSeat(Interactor interactor)
        {
            base.TakeSeat(interactor);

            //This grabs a reference to the dynamic version of the space ship
            //In other words the dynamic space ship is the one that moves and rotates
            GameObject otherSpaceShip = GetComponentInParent<GameObjectReference>().Reference;

            AbstractMovement movement = otherSpaceShip.GetComponent<AbstractMovement>();
            AbstractAim aim = otherSpaceShip.GetComponent<AbstractAim>();
            movement.enabled = true;
            aim.enabled = true;
        }

        protected override void LeaveSeat(Interactor interactor)
        {
            base.LeaveSeat(interactor);
            
            //This grabs a reference to the dynamic version of the space ship
            //In other words the dynamic space ship is the one that moves and rotates
            GameObject otherSpaceShip = GetComponentInParent<GameObjectReference>().Reference;

            AbstractMovement movement = otherSpaceShip.GetComponent<AbstractMovement>();
            AbstractAim aim = otherSpaceShip.GetComponent<AbstractAim>();
            movement.enabled = false;
            aim.enabled = false;
        }
    }
}