using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.CameraUtilities
{
    public class CameraScrolling : MonoBehaviour
    {
        [SerializeField] private float scrollInLimit;
        [SerializeField] private float scrollOutLimit;
        [SerializeField] [Range(1f, 10f)] private float intensity;

        private Camera cam;
        private Coroutine coLerpSize;
        private float currentSize;
        private bool isScrolling;

        private void Start()
        {
            cam = GetComponent<Camera>();
            currentSize = cam.orthographicSize;
            isScrolling = false;
        }

        private void Update()
        {
            float scrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");

            if(scrollWheel != 0f)
            {
                if(isScrolling) StopCoroutine(coLerpSize);

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
            isScrolling = true;

            while(currentSize != endValue)
            {
                currentSize = Mathf.Lerp(currentSize, endValue, intensity * Time.deltaTime);
                cam.orthographicSize = currentSize;
                yield return null;
            }

            isScrolling = false;
            StopCoroutine(coLerpSize);
        }

        private void OnDisable()
        {
            StopCoroutine(coLerpSize);
        }
    }
}