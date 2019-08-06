using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Utility;
using SeizeCommand.Networking;

namespace SeizeCommand.Health
{
    public class OnDiePlayFadeDeathScreenAnimation : AbstractEventSubscriber<HealthManager>
    {
        [SerializeField] private Animator anim;
        [SerializeField] private CanvasPanelReferences references;
        private NetworkIdentity networkIdentity;

        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
        }

        private void OnEnable()
        {
            item.OnDie += FadeDeathScreen;
        }

        private void OnDisable()
        {
            item.OnDie -= FadeDeathScreen;
        }

        private void FadeDeathScreen()
        {
            if(networkIdentity.IsLocalPlayer)
            {
                anim.SetTrigger("Die");
            }
        }
    }
}