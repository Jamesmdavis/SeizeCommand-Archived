using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactables;

namespace SeizeCommand.Interactions.Interactors
{
    public class Interactor : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField] private GameObject player;

        public GameObject Player
        {
            get { return player; }
        }

        public event Action OnInteract;

        protected List<IInteractable> interactables;
        private IInteractable currentInteractable;

        private void Start()
        {
            player = player == null ? gameObject : player;
            interactables = new List<IInteractable>();
        }

        protected virtual void Update()
        {
            CheckInteract();
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

        protected virtual void Interact()
        {
            if(OnInteract != null) OnInteract();

            interactables[0].Interact(this);
        }

        protected void CheckInteract()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(interactables.Count != 0)
                {
                    Interact();
                }
            }
        }
    }
}