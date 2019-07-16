using UnityEngine;

using SeizeCommand.Interactions.Interactors;

namespace SeizeCommand.Interactions.Interactables
{
    public interface IInteractable
    {
        void Interact(PlayerInteractor interactor);
    }
}