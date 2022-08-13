using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Signals;

public class DronePoolPhysicsController: MonoBehaviour
{
    public bool isReady = true;
    private Transform playerTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collected") && isReady)
        {
            //StopAllCoroutines();
            playerTransform = other.transform;
           
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isReady = true;
            GunPoolSignals.Instance.onWrongGunPoolExit?.Invoke();
        }
    }
}
