using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Utility
{
    public class LifeTime : MonoBehaviour
    {
        [SerializeField] private float time;
        private Coroutine coLifeTime;

        private void Start()
        {
            coLifeTime = StartCoroutine(CoLifeTime(time));
        }

        private IEnumerator CoLifeTime(float sec)
        {
            yield return new WaitForSeconds(sec);
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            StopCoroutine(coLifeTime);
        }
    }
}