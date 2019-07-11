using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.DamageSenders
{
    public abstract class AbstractDamageSender : MonoBehaviour
    {
        [SerializeField] protected float damage;

        protected virtual void OnCollisionEnter2D(Collision2D coll)
        {
            Destroy(gameObject);
            SendDamage();
        }


        protected abstract void SendDamage();
    }
}