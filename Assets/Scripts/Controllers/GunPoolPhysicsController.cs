using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Signals;
using Controllers;

public class GunPoolPhysicsController: MonoBehaviour
{

    #region vars
    #region public vars

    #endregion
    #region serializefield vars
    [SerializeField] TurretController turretController;
    #endregion
    #region private vars
    private bool _isReady = true;
    private float _reloadTime = 0.5f;

    private Transform playerTransform;
    #endregion
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _isReady)
        {
            //StopAllCoroutines();
            playerTransform = other.transform;
            StartCoroutine(FireAndReload());
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _isReady)
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            _isReady = true;
            GunPoolSignals.Instance.onWrongGunPoolExit?.Invoke();
        }
    }



    IEnumerator FireAndReload()
    {
        GunPoolSignals.Instance.onWrongGunPool?.Invoke(playerTransform);
        turretController.RotateToPlayer(playerTransform);
        _isReady = false;
        yield return new WaitForSeconds(_reloadTime);
        _isReady = true;
        StartCoroutine(FireAndReload());
    }
}
