using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.CameraUtilities
{
    public class CameraRotateWithShip : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private float cameraSize;
        [SerializeField] [Range(1f, 10f)] private float intensity;


        [Header("Object References")]
        [SerializeField] private GameObject ship;

        private Camera cam;
        private bool hasReturned;

        private void Start()
        {
            cam = GetComponent<Camera>();
            hasReturned = false;
        }

        private void Update()
        {
            if(cam.orthographicSize <= cameraSize)
            {
                Quaternion currentRotation = transform.rotation;
                Quaternion endRotation = ship.transform.rotation;

                Quaternion newRotation = Quaternion.Slerp(currentRotation, endRotation,
                    intensity * Time.deltaTime);

                transform.rotation = newRotation;
            }
            else
            {
                if(!hasReturned)
                {
                    Quaternion currentRotation = transform.rotation;

                    Quaternion newRotation = Quaternion.Slerp(currentRotation, Quaternion.identity, 
                        intensity * Time.deltaTime);

                    transform.rotation = newRotation;
                }
            }

            hasReturned = transform.rotation == Quaternion.identity ? true : false;
        }
    }
}