using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace SeizeCommand.Utility
{
    public class TurnOnButtonAnimationEvent : MonoBehaviour
    {
        [SerializeField] private Button button;

        public void Event()
        {
            button.interactable = true;
        }
    }
}