using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Health;

namespace SeizeCommand.DamageSenders
{
    public abstract class AbstractDamageSender : MonoBehaviour
    {
        [SerializeField] protected float damage;

        public GameObject Sender
        {
            get;
            set;
        }

        protected virtual void OnTriggerEnter2D(Collider2D coll)
        {
            if(coll.gameObject.GetComponent(typeof(IDamageable)))
            {
                IDamageable damageable = coll.gameObject.GetComponent<IDamageable>();
                SendDamage(damageable);
            }

            Destroy(gameObject);
        }

        protected abstract void SendDamage(IDamageable damageable);
    }
}