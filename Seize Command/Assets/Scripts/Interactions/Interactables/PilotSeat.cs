using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Movement;
using SeizeCommand.Aiming;
using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Attack;
using SeizeCommand.References;
using SeizeCommand.Networking;

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

            NetworkForceMovement movement = otherSpaceShip.GetComponent<NetworkForceMovement>();
            movement.NetworkIdentity = CurrentInteractor.Player.GetComponent<NetworkIdentity>();

            NetworkSlerpAim aim = otherSpaceShip.GetComponent<NetworkSlerpAim>();
            aim.NetworkIdentity = CurrentInteractor.Player.GetComponent<NetworkIdentity>();
        }

        protected override void LeaveSeat(Interactor interactor)
        {
            base.LeaveSeat(interactor);
            
            //This grabs a reference to the dynamic version of the space ship
            //In other words the dynamic space ship is the one that moves and rotates
            GameObject otherSpaceShip = GetComponentInParent<GameObjectReference>().Reference;

            NetworkForceMovement movement = otherSpaceShip.GetComponent<NetworkForceMovement>();
            movement.NetworkIdentity = null;

            NetworkSlerpAim aim = otherSpaceShip.GetComponent<NetworkSlerpAim>();
            aim.NetworkIdentity = null;
        }
    }
}