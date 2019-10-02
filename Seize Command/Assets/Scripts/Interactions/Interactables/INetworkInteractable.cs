using SeizeCommand.Interactions.Interactors;

namespace SeizeCommand.Interactions.Interactables
{
    public interface INetworkInteractable
    {
        void RPCInteract(Interactor interactor);
    }
}