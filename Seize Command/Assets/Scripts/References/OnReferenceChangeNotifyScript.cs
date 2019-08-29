using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Utility;

namespace SeizeCommand.References
{
    //This class is an event subscriber
    //This class sets the player reference for all of the scripts on the camera that need to know
    //about the player
    //I am using an Interface call IReferenceable to allow me to use the same method for multiple scripts
    public class OnReferenceChangeNotifyScript : AbstractEventSubscriber<GameObjectReference>
    {
        private IReferenceable[] scripts;

        protected override void Awake()
        {
            base.Awake();
            scripts = GetComponentsInParent<IReferenceable>();
        }

        private void OnEnable()
        {
            item.OnReferenceChange += NotifyScripts;
        }

        private void OnDisable()
        {
            item.OnReferenceChange -= NotifyScripts;
        }

        private void NotifyScripts(GameObject reference)
        {
            foreach(IReferenceable script in scripts)
            {
                script.SetReference(reference);
            }
        }
    }
}