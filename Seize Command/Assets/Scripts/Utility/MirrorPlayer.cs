using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.References;

namespace SeizeCommand.Utility
{
    //This script Mirrors the Dynamic Players Movement and Rotation
    [RequireComponent(typeof(GameObjectReference))]
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
                    player = GetComponent<GameObjectReference>().Reference.transform;
                }
                else
                {
                    if(mirrorPosition)
                    {
                        transform.localPosition = player.localPosition;
                    }
                    if(mirrorRotation)
                    {
                        transform.localRotation = player.localRotation;
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