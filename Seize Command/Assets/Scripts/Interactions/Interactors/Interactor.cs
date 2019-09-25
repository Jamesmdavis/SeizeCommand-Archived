using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactables;
using SeizeCommand.Utility;

namespace SeizeCommand.Interactions.Interactors
{
    public class Interactor : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField] private GameObject player;

        private IInteractable currentInteractable;
        protected List<IInteractable> interactables;
        protected InputManager controller;

        public GameObject Player
        {
            get { return player; }
        }

        public IInteractable CurrentInteractable 
        {
            get { return currentInteractable; }
            set { currentInteractable = value; }
        }

        public virtual InputManager Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        public event Action OnInteract;

        private void Start()
        {
            player = player == null ? gameObject : player;
            interactables = new List<IInteractable>();
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if(coll.GetComponent(typeof(IInteractable)))
            {
                IInteractable i = coll.GetComponent<IInteractable>();

                if(!interactables.Contains(i))
                {
                    interactables.Add(i);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D coll)
        {
            if(coll.GetComponent(typeof(IInteractable)))
            {
                IInteractable i = coll.GetComponent<IInteractable>();

                if(interactables.Contains(i))
                {
                    interactables.Remove(i);
                }
            }
        }

        protected virtual void Interact(IInteractable interactable)
        {  
            interactable.Interact(this);
            if(OnInteract != null) OnInteract();
        }

        public virtual void CheckInput()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(currentInteractable != null) Interact(currentInteractable);
                else if(interactables.Count != 0)
                {
                    currentInteractable = interactables[0];
                    Interact(currentInteractable);
                }
            }
        }
    }
}