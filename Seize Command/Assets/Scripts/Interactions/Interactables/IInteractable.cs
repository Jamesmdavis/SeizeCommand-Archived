using UnityEngine;

namespace SeizeCommand.Interactions.Interactables
{
    public interface IInteractable
    {
        void Interact(GameObject interactor);
    }
}