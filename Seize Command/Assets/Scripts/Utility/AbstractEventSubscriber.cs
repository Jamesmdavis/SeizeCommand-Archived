using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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