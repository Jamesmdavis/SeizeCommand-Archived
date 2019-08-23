using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Utility
{
    [RequireComponent(typeof(PlayerReference))]
    public class MirrorPlayer : MonoBehaviour
    {
        [SerializeField] private bool mirrorPosition;
        [SerializeField] private bool mirrorRotation;

        private Transform player;
        private Coroutine coMirrorPlayer;

        private void Start()
        {
            coMirrorPlayer = StartCoroutine(CoMirrorPlayer());
        }

        private IEnumerator CoMirrorPlayer()
        {
            while(true)
            {
                if(!player)
                {
                    player = GetComponent<PlayerReference>().Reference.transform;
                }
                else
                {
                    if(mirrorPosition)
                    {
                        transform.localPosition = player.localPosition;
                    }
                    if(mirrorRotation)
                    {
                        transform.rotation = player.rotation;
                    }
                }
                
                yield return null;
            }
        }

        private void OnDisable()
        {
            StopCoroutine(coMirrorPlayer);
        }
    }
}