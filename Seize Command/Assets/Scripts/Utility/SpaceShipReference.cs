using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeizeCommand.Utility
{
    public class SpaceShipReference : MonoBehaviour
    {
        [SerializeField] private GameObject spaceShip;

        public GameObject Reference { get { return spaceShip; } }
    }
}