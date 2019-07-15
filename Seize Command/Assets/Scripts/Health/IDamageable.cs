using UnityEngine;

namespace SeizeCommand.Health
{
    public interface IDamageable
    {
        void TakeDamage(GameObject sender, float damage);
    }
}