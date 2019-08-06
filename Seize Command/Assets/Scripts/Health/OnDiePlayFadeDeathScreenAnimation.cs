using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Utility;

namespace SeizeCommand.Health
{
    public class OnDiePlayFadeDeathScreenAnimation : AbstractEventSubscriber<HealthManager>
    {
        [SerializeField] private Animator anim;

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
            anim.SetTrigger("Die");
        }
    }
}