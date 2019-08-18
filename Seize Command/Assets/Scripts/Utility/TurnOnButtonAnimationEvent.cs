using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace SeizeCommand.Utility
{
    public class TurnOnButtonAnimationEvent : MonoBehaviour
    {
        [SerializeField] private Button[] buttons;

        public void Event()
        {
            foreach(Button b in buttons)
            {
                b.interactable = true;
            }
        }
    }
}