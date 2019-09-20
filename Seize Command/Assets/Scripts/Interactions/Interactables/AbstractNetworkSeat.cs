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
                SeatMove seatMove = new SeatMove();
                seatMove.position = new Vector2Data();
                seatMove.position.x = transform.position.x;
                seatMove.position.y = transform.position.y;
                seatMove.rotation = 180f;

                networkIdentity.Socket.Emit("seatMove", new JSONObject(JsonUtility.ToJson(seatMove)));
            }
        }
    }
}