using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Attack;
using SeizeCommand.Aiming;
using SeizeCommand.Utility;
using SeizeCommand.References;

namespace SeizeCommand.Interactions.Interactables
{
    public class GunSeat : AbstractNetworkSeat
    {
        [SerializeField] private GameObject weaponSlot;

        protected override void TakeSeat(Interactor interactor)
        {
            base.TakeSeat(interactor);

            InputManager input = CurrentInteractor.Player.GetComponent<InputManager>();

            AttackManager attackManager = weaponSlot.GetComponent<AttackManager>();
            input.AttackScript = attackManager;

            AbstractAim aim = weaponSlot.GetComponent<AbstractAim>();
            input.AimScript = aim;
        }

        protected override void LeaveSeat(Interactor interactor)
        {
            InputManager input = CurrentInteractor.Player.GetComponent<InputManager>();

            AttackManager attackManager = CurrentInteractor.Player.GetComponent<AttackManager>();
            input.AttackScript = attackManager;

            References<GameObject> references = CurrentInteractor.Player
                .GetComponent<References<GameObject>>();
            GameObject otherPlayer = references.GetReferenceByName("Other Player");

            AbstractAim aim = otherPlayer.GetComponent<AbstractAim>();
            input.AimScript = aim;

            base.LeaveSeat(interactor);
        }
    }
}