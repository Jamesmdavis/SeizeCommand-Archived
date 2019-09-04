using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Movement;
using SeizeCommand.Aiming;
using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Attack;
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

            NetworkShipMovement movement = otherSpaceShip.GetComponent<NetworkShipMovement>();
            movement.Pilot = CurrentInteractor.Player;
            movement.enabled = true;

            AbstractAim aim = otherSpaceShip.GetComponent<AbstractAim>();
            aim.enabled = true;

            AttackManager playerAttack = interactor.Player.GetComponent<AttackManager>();
            playerAttack.enabled = false;
        }

        protected override void LeaveSeat(Interactor interactor)
        {
            base.LeaveSeat(interactor);
            
            //This grabs a reference to the dynamic version of the space ship
            //In other words the dynamic space ship is the one that moves and rotates
            GameObject otherSpaceShip = GetComponentInParent<GameObjectReference>().Reference;

            NetworkShipMovement movement = otherSpaceShip.GetComponent<NetworkShipMovement>();
            movement.Pilot = null;
            movement.enabled = false;

            AbstractAim aim = otherSpaceShip.GetComponent<AbstractAim>();
            aim.enabled = false;

            AttackManager playerAttack = interactor.Player.GetComponent<AttackManager>();
            playerAttack.enabled = false;
        }
    }
}