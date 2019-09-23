using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Utility;

namespace SeizeCommand.Referencing
{
    //This class is an event subscriber
    //This class sets the player reference for all of the scripts on the camera that need to know
    //about the player
    //I am using an Interface call IReferenceable to allow me to use the same method for multiple scripts
    public class OnReferenceChangeNotifyScript<T> : AbstractEventSubscriber<References<T>>
    {
        private IReferenceable<T>[] scripts;

        protected override void Awake()
        {
            base.Awake();
            scripts = GetComponentsInParent<IReferenceable<T>>();
        }

        private void OnEnable()
        {
            item.OnReferenceChange += NotifyScripts;
        }

        private void OnDisable()
        {
            item.OnReferenceChange -= NotifyScripts;
        }

        private void NotifyScripts(ReferenceData<T> referenceData)
        {
            foreach(IReferenceable<T> script in scripts)
            {
                script.SetReference(referenceData);
            }
        }
    }
}