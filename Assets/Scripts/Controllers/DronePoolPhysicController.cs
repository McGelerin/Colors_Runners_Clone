using System;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Controllers
{
    public class DronePoolPhysicController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private DronePoolManager manager;
        
        #endregion

        #region Private Variables

        private int _triggerCache=0;

        #endregion
        #endregion
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.SelectedArea = true;
                return;
            }

            if (other.CompareTag("Collected") && _triggerCache<=0)
            {
                manager.SetTruePoolColor(other.transform.parent.GetComponent<CollectableManager>().ColorState);
                _triggerCache++;
            }
        }
    }
}