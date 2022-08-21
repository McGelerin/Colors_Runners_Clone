using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Signals;
using Controllers;

public class GunPoolPhysicsController: MonoBehaviour
{

    #region vars
    #region public vars
    public bool IsTrue = false;
    //public Transform playerTransform;

    #endregion
    #region serializefield vars
    [SerializeField] GunPoolManager manager;
    #endregion
    #region private vars
    //private bool _isReady = true;
    //private float _reloadTime = 0.5f;

    #endregion
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsTrue.Equals(true))
            {
                manager.StopCoroutineManager();
            }
            else
            {
                manager.StartCoroutineManager();
            }
        }
    }
}
