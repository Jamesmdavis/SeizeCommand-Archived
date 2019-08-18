using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Utility
{
    public class CameraFollowPlayer : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private float zCameraOffset;

        private Transform playerTranform;
        private Vector3 newCameraPosition;

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

        public void SetFollowPlayer(Transform transform)
        {
            playerTranform = transform;
        }
    }
}