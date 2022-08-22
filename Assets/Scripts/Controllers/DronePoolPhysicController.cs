using System;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Controllers
{
    public class DronePoolPhysicController : MonoBehaviour
    {
        [SerializeField] private DronePoolManager manager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.SelectedArea = true;
            }
        }
    }
}