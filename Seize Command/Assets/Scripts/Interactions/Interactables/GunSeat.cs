using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Utility;
using SeizeCommand.Attack;
using SeizeCommand.Weapon;

namespace SeizeCommand.Interactions.Interactables
{
    public class GunSeat : AbstractNetworkSeat
    {
        [SerializeField] private AbstractGun gun;

        protected override void TakeSeat(Interactor interactor)
        {
            base.TakeSeat(interactor);

            //This grabs a reference to the dynamic version of the space ship
            //In other words the dynamic space ship is the one that moves and rotates
            GameObject otherSpaceShip = GetComponentInParent<SpaceShipReference>().Reference;

            AttackManager playerAttack = interactor.Player.GetComponent<AttackManager>();
            playerAttack.CurrentWeapon = gun;
        }

        protected override void LeaveSeat(Interactor interactor)
        {
            base.TakeSeat(interactor);

            //This grabs a reference to the dynamic version of the space ship
            //In other words the dynamic space ship is the one that moves and rotates
            GameObject otherSpaceShip = GetComponentInParent<SpaceShipReference>().Reference;

            AttackManager playerAttack = interactor.Player.GetComponent<AttackManager>();
            playerAttack.DefaultWeapon();
        }
    }
}