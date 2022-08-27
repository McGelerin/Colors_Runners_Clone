using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class IdleCarPhysicsController : MonoBehaviour
{
    #region Self Variables
    #region Public Variables


    #endregion
    #region SerializeField Variables

    #endregion
    #region Private Variables

    private IdleCarManager _manager;
    private bool _isOnTargetTrigger = false;


    #endregion
    #endregion


    private void Awake()
    {
        _manager = transform.parent.GetComponent<IdleCarManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            _isOnTargetTrigger = true;
            _manager.SelectRandomDirection(other.GetComponent<IdleCarTargetController>().GetData());
            return;
        }
        if (other.CompareTag("Player"))
        {
            _manager.PlayerOnRoad();
            return;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            _isOnTargetTrigger = false;
            return;

        }
        if (other.CompareTag("Player"))
        {
            //_manager.Move();
            _manager.MoveAfterPlayer(_isOnTargetTrigger);
            return;
        }
    }
}
