using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Interactions.Interactables
{
    public class BoxInteractable : MonoBehaviour, IInteractable
    {
        public void Interact(GameObject interactor)
        {
            Debug.Log("Interact");
        }
    }
}