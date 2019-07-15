using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Interactions.Interactables;

namespace SeizeCommand.Interactions.Interactors
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField] private GameObject player;

        private List<IInteractable> interactables;
        private IInteractable currentInteractable;

        private void Start()
        {
            player = player == null ? gameObject : player;
            interactables = new List<IInteractable>();
        }

        private void FixedUpdate()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
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

        private void Interact()
        {
            if(interactables.Count != 0)
            {
                interactables[0].Interact(player);
                interactables.RemoveAt(0);
            }
        }
    }
}