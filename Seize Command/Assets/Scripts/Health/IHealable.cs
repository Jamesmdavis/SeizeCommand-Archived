using UnityEngine;

namespace SeizeCommand.Health
{
    public interface IHealable
    {
        void Heal(GameObject sender, float heal);
    }
}