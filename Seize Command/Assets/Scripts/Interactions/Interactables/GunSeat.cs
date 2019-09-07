using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Attack;
using SeizeCommand.Aiming;
using SeizeCommand.Networking;

namespace SeizeCommand.Interactions.Interactables
{
    public class GunSeat : AbstractNetworkSeat
    {
        [SerializeField] private GameObject weaponSlot;

        protected override void TakeSeat(Interactor interactor)
        {
            base.TakeSeat(interactor);

            NetworkAttackManager attackManager = weaponSlot.GetComponent<NetworkAttackManager>();
            attackManager.NetworkIdentity = CurrentInteractor.Player.GetComponent<NetworkIdentity>();

            NetworkMouseAim aim = weaponSlot.GetComponent<NetworkMouseAim>();
            aim.NetworkIdentity = CurrentInteractor.Player.GetComponent<NetworkIdentity>();
        }

        protected override void LeaveSeat(Interactor interactor)
        {
            base.LeaveSeat(interactor);

            NetworkAttackManager attackManager = weaponSlot.GetComponent<NetworkAttackManager>();
            attackManager.NetworkIdentity = null;

            NetworkMouseAim aim = weaponSlot.GetComponent<NetworkMouseAim>();
            aim.NetworkIdentity = null;
        }
    }
}