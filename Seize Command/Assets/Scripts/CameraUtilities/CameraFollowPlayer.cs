using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.References;

namespace SeizeCommand.CameraUtilities
{
    public class CameraFollowPlayer : MonoBehaviour, IReferenceable
    {
        [Header("Data")]
        [SerializeField] private float zCameraOffset;

        private Transform playerTranform;
        private Vector3 newCameraPosition;
        private Coroutine coFindPlayer;

        private void Start()
        {
            playerTranform = null;
            newCameraPosition = new Vector3(0, 0, zCameraOffset);
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

        public void SetReference(GameObject reference)
        {
            playerTranform = reference.transform;
        }
    }
}