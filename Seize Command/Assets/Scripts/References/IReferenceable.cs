using UnityEngine;

namespace SeizeCommand.References
{
    public interface IReferenceable<T>
    {
        void SetReference(ReferenceData<T> reference);
    }
}