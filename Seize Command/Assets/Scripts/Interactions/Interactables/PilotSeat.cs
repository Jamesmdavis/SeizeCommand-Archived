using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Movement;
using SeizeCommand.Aiming;
using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Referencing;
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
            References<GameObject> playerReferences = CurrentInteractor.Player
                .GetComponent<References<GameObject>>();
            GameObject staticShip = playerReferences.GetReferenceByName("Static Space Ship");

            References<GameObject> staticShipReferences = staticShip.GetComponent<References<GameObject>>();
            GameObject dynamicShip = staticShipReferences.GetReferenceByName("Dynamic Space Ship");

            AbstractMovement movement = dynamicShip.GetComponent<AbstractMovement>();
            input.MovementScript = movement;

            AbstractAim aim = dynamicShip.GetComponent<AbstractAim>();
            input.AimScript = aim;
        }

        protected override void LeaveSeat(Interactor interactor)
        {   
            InputManager input = CurrentInteractor.Player.GetComponent<InputManager>();

            AbstractMovement movement = CurrentInteractor.Player.GetComponent<AbstractMovement>();
            input.MovementScript = movement;

            References<GameObject> references = CurrentInteractor.Player.GetComponent<References<GameObject>>();
            GameObject otherPlayer = references.GetReferenceByName("Other Player");

            AbstractAim aim = otherPlayer.GetComponent<AbstractAim>();
            input.AimScript = aim;
            
            base.LeaveSeat(interactor);
        }
    }
}