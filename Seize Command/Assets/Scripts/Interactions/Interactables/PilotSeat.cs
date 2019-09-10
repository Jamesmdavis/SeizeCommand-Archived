using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Movement;
using SeizeCommand.Aiming;
using SeizeCommand.Interactions.Interactors;
using SeizeCommand.References;
using SeizeCommand.Networking;
using SeizeCommand.Utility;

namespace SeizeCommand.Interactions.Interactables
{
    public class PilotSeat : AbstractNetworkSeat
    {
        protected override void TakeSeat(Interactor interactor)
        {
            base.TakeSeat(interactor);

            InputManager input = CurrentInteractor.Player.GetComponent<InputManager>();

            //This grabs a reference to the dynamic version of the space ship
            //In other words the dynamic space ship is the one that moves and rotates
            GameObject otherSpaceShip = GetComponentInParent<GameObjectReference>().Reference;

            AbstractMovement movement = otherSpaceShip.GetComponent<AbstractMovement>();
            input.MovementScript = movement;

            AbstractAim aim = otherSpaceShip.GetComponent<AbstractAim>();
            input.AimScript = aim;
        }

        protected override void LeaveSeat(Interactor interactor)
        {   
            InputManager input = CurrentInteractor.Player.GetComponent<InputManager>();

            AbstractMovement movement = CurrentInteractor.Player.GetComponent<AbstractMovement>();
            input.MovementScript = movement;

            AbstractAim aim = CurrentInteractor.Player.GetComponent<GameObjectReference>()
                .Reference.GetComponent<AbstractAim>();
            input.AimScript = aim;
            
            base.LeaveSeat(interactor);
        }
    }
}