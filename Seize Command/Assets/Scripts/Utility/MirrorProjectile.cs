using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Utility
{
    public class MirrorProjectile : MonoBehaviour
    {
        [SerializeField] private bool mirrorPosition;
        [SerializeField] private bool mirrorRotation;

        private Transform projectile;
        private Coroutine coMirrorProjectile;
        private bool isMirroring;

        private IEnumerator CoMirrorProjectile()
        {
            isMirroring = true;

            while(true)
            {
                if(!projectile)
                {
                    isMirroring = false;
                    StopCoroutine(coMirrorProjectile);
                    Destroy(gameObject);
                }
                else
                {
                    transform.localPosition = mirrorPosition ? projectile.localPosition 
                        : transform.localPosition;

                    transform.rotation = mirrorRotation ? projectile.rotation 
                        : transform.rotation;
                }

                yield return null;
            }
        }

        public void SetMirrorTarget(Transform target)
        {
            projectile = target;
            coMirrorProjectile = StartCoroutine(CoMirrorProjectile());
        }

        private void OnDisable()
        {
            if(isMirroring) StopCoroutine(coMirrorProjectile);
        }
    }
}