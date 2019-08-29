using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Utility;

namespace SeizeCommand.Weapon
{
    public class OnInstantiateProjectileMirrorProjectile : AbstractEventSubscriber<Gun>
    {
        private PlayerProjectileTracker tracker;

        protected override void Awake()
        {
            base.Awake();
            tracker = GetComponentInParent<PlayerProjectileTracker>();
        }

        private void OnEnable()
        {
            item.OnInstantiateProjectile += SendProjToTracker;
        }

        private void OnDisable()
        {
            item.OnInstantiateProjectile -= SendProjToTracker;
        }

        private void SendProjToTracker(GameObject proj)
        {
            tracker.CreateMirrorProjectile(proj);
        }
    }
}