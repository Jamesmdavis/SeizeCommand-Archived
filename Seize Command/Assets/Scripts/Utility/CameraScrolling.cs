using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Utility
{
    public class CameraScrolling : MonoBehaviour
    {
        [SerializeField] private float scrollInLimit;
        [SerializeField] private float scrollOutLimit;
        [SerializeField] [Range(0f, 1f)] private float intensity;

        private Camera cam;
        private Coroutine coLerpSize;
        private float currentSize;
        private bool isLerping;

        private void Start()
        {
            cam = GetComponent<Camera>();
            currentSize = cam.orthographicSize;
            isLerping = false;
        }

        private void Update()
        {
            float scrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");

            if(scrollWheel != 0f)
            {
                if(isLerping) StopCoroutine(coLerpSize);

                if(scrollWheel > 0f)
                {
                    coLerpSize = StartCoroutine(CoLerpSize(scrollInLimit));
                }
                else if(scrollWheel < 0f)
                {
                    coLerpSize = StartCoroutine(CoLerpSize(scrollOutLimit));
                }
            }
        }

        private IEnumerator CoLerpSize(float endValue)
        {
            isLerping = true;

            while(currentSize != endValue)
            {
                currentSize = Mathf.Lerp(currentSize, endValue, intensity * Time.deltaTime);
                cam.orthographicSize = currentSize;
            }

            yield return null;

            isLerping = false;

            StopCoroutine(coLerpSize);
        }

        private void OnDisable()
        {
            StopCoroutine(coLerpSize);
        }
    }
}