using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Attack;

namespace SeizeCommand.Utility
{
    public class AbstractEventSubscriber <T> : MonoBehaviour
    {
        protected T item;

        protected virtual void Awake()
        {
            item = GetComponent<T>();
        }
    }
}