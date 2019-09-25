using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Attack;
using SeizeCommand.Aiming;
using SeizeCommand.Utility;
using SeizeCommand.Referencing;
using SeizeCommand.Networking;

namespace SeizeCommand.Interactions.Interactables
{
    public class GunSeat : AbstractSeat
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

            SendData(interactor, transform.position);
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

            SendData(interactor, leaveSeatPosition.position);
        }

        private void SendData(Interactor interactor, Vector3 position)
        {
            NetworkIdentity networkIdentity = interactor.Player.GetComponent<NetworkIdentity>();

            if(networkIdentity.IsLocalPlayer)
            {
                Vector2Package changePositionPackage = new Vector2Package();
                changePositionPackage.id = networkIdentity.ID;
                changePositionPackage.vector2 = new Vector2Data();
                changePositionPackage.vector2.x = position.x;
                changePositionPackage.vector2.y = position.y;

                RotationPackage changeRotationPackage = new RotationPackage();
                changeRotationPackage.id = networkIdentity.ID;
                changeRotationPackage.rotation = 180f;

                networkIdentity.Socket.Emit("changePosition", 
                    new JSONObject(JsonUtility.ToJson(changePositionPackage)));

                networkIdentity.Socket.Emit("changeRotation",
                    new JSONObject(JsonUtility.ToJson(changeRotationPackage)));
            }
        }
    }
}