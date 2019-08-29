using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.CameraUtilities
{
    //This class will fade the roof of the Dynamic Space Ship given a certain "fadeValue"
    //"fadeValue" is a value that is compared to the current size of the camera
    //If the camera's size is equal to the "fadeValue" then the roof of the space ship will either
    //fade in or fade out depending on whether or not it is currently faded in or out
    public class FadeRoofWithCameraSize : MonoBehaviour
    {
        [SerializeField] private float fadeValue;
        [SerializeField] [Range(0f, 1f)] private float fadeInLimit;
        [SerializeField] [Range(0f, 1f)] private float fadeOutLimit;
        [SerializeField] [Range(1f, 10f)] private float intensity;

        //FadeState represents the current state of the roof
        private enum FadeState
        {
            FadedIn,
            FadedOut
        }

        private FadeState fadeState;
        private Camera mainCamera;
        private List<SpriteRenderer> roofSprites;
        private float currentAlphaColorValue;
        private Coroutine coFadeRoof;
        private bool isFading;

        private void Start()
        {
            fadeState = FadeState.FadedOut;
            mainCamera = Camera.main;

            //These next few lines of code grab all the roof sprites that will be faded
            roofSprites = new List<SpriteRenderer>();
            SpriteRenderer[] childSprites = GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer sprite in childSprites)
            {
                if(sprite.sortingLayerName == "Roof") roofSprites.Add(sprite);
            }

            currentAlphaColorValue = 255f;
            isFading = false;
        }

        private void Update()
        {
            if(fadeState == FadeState.FadedOut)
            {
                if(mainCamera.orthographicSize < fadeValue)
                {
                    //Fade In
                    if(isFading) StopCoroutine(coFadeRoof);
                    coFadeRoof = StartCoroutine(CoFadeRoof(fadeInLimit));
                    fadeState = FadeState.FadedIn;
                }
            }
            else if(fadeState == FadeState.FadedIn)
            {
                if(mainCamera.orthographicSize > fadeValue)
                {
                    //Fade Out
                    if(isFading) StopCoroutine(coFadeRoof);
                    coFadeRoof = StartCoroutine(CoFadeRoof(fadeOutLimit));
                    fadeState = FadeState.FadedOut;
                }
            }
        }

        private IEnumerator CoFadeRoof(float endValue)
        {   
            isFading = true;

            while(currentAlphaColorValue != endValue)
            {
                foreach(SpriteRenderer sprite in roofSprites)
                {
                    Color currentColor = sprite.color;
                    Color endColor = sprite.color;
                    endColor.a = endValue;

                    currentColor = Color.Lerp(currentColor, endColor, intensity * Time.deltaTime);
                    sprite.color = currentColor;
                }

                currentAlphaColorValue = roofSprites[0].color.a;
                yield return null;
            }

            isFading = false;
            StopCoroutine(coFadeRoof);
        }

        private void OnDisable()
        {
            if(isFading) StopCoroutine(coFadeRoof);
        }
    }
}
