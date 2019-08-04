using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Utility;

namespace SeizeCommand.Health
{
    public class OnTakeDamagePlayTakeDamageAnimation : AbstractEventSubscriber<HealthManager>
    {
        private Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            item.OnTakeDamage += PlayTakeDamageAnimation;
        }

        private void OnDisable()
        {
            item.OnTakeDamage -= PlayTakeDamageAnimation;
        }

        private void PlayTakeDamageAnimation()
        {
            anim.SetTrigger("Take Damage");
        }
    }
}