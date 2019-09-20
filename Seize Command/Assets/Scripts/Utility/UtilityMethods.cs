using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Utility
{
    public static class UtilityMethods
    {
        public static GameObject FindGameObjectInParentByName(string name, Transform currentObject)
        {
            if(currentObject.name == name) return currentObject.gameObject;
            Transform parent = currentObject.transform.parent;
            return parent != null ? FindGameObjectInParentByName(name, parent) : null;
        }
    }
}