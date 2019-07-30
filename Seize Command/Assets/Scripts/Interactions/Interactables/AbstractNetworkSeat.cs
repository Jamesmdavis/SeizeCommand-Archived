using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactors;
using SeizeCommand.Networking;

namespace SeizeCommand.Interactions.Interactables
{
    public abstract class AbstractNetworkSeat : AbstractSeat
    {
        protected override void TakeSeat(Interactor interactor)
        {
            base.TakeSeat(interactor);
            SendData(interactor, transform);
        }

        protected override void LeaveSeat(Interactor interactor)
        {
            SendData(interactor, leaveSeatPosition);
            base.LeaveSeat(interactor);
        }

        private void SendData(Interactor interactor, Transform transform)
        {
            NetworkIdentity networkIdentity = interactor.Player.GetComponent<NetworkIdentity>();

            if(networkIdentity.IsLocalPlayer)
            {
                SeatUpdatePositionRotation seatUpdatePositionRotation = new SeatUpdatePositionRotation();
                seatUpdatePositionRotation.position = new Position();
                seatUpdatePositionRotation.position.x = transform.position.x;
                seatUpdatePositionRotation.position.y = transform.position.y;
                seatUpdatePositionRotation.rotation = 180f;

                networkIdentity.Socket.Emit("seatUpdatePositionRotation", new JSONObject(JsonUtility.ToJson(seatUpdatePositionRotation)));
            }
        }
    }
}