using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Referencing;

namespace SeizeCommand.CameraUtilities
{
    public class CameraFollowPlayer : MonoBehaviour, IReferenceable<Transform>
    {
        private Transform playerTranform;
        private Vector3 newCameraPosition;

        private void Start()
        {
            playerTranform = null;
            newCameraPosition = new Vector3(0, 0, -10f);
        }

        private void LateUpdate()
        {
            if(playerTranform != null)
            {
                newCameraPosition.x = playerTranform.position.x;
                newCameraPosition.y = playerTranform.position.y;
                transform.position = newCameraPosition;
            }
        }

        public void SetReference(ReferenceData<Transform> referenceData)
        {
            playerTranform = referenceData.Reference;
        }
    }
}