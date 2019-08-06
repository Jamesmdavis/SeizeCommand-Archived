using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeizeCommand.Utility
{
    public class CanvasPanelReferences : MonoBehaviour
    {
        [SerializeField] private GameObject[] panels;

        public GameObject GetPanel(string panelName)
        {
            foreach(GameObject panel in panels)
            {
                if(panel.name == panelName)
                {
                    return panel;
                }
            }

            return null;
        }
    }
}