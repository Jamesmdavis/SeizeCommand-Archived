using UnityEngine;

namespace SeizeCommand.Referencing
{
    public interface IReferenceable
    {
        void SetReference(ReferenceData reference);
    }
}