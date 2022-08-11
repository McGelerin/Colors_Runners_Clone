using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Signals;

public class GunPoolPhysicsController: MonoBehaviour
{
    public bool isReady = true;
    private float reloadTime = 0.5f;

    private Transform playerTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isReady)
        {
            //StopAllCoroutines();
            playerTransform = other.transform;
            StartCoroutine(Reload());
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isReady)
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            isReady = true;
            GunPoolSignals.Instance.onWrongGunPoolExit?.Invoke();
        }
    }



    IEnumerator Reload()
    {
        GunPoolSignals.Instance.onWrongGunPool?.Invoke(playerTransform);

        isReady = false;
        yield return new WaitForSeconds(reloadTime);
        isReady = true;
        StartCoroutine(Reload());
    }
}
