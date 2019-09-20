using UnityEngine;

namespace SeizeCommand.Referencing
{
    public interface IReferenceable<T>
    {
        void SetReference(ReferenceData<T> reference);
    }
}