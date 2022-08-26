using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class IdleCarPhysicsController : MonoBehaviour
{
    private IdleCarManager _manager;

    private void Awake()
    {
        _manager = transform.parent.GetComponent<IdleCarManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            _manager.SelectRandomDirection(other.GetComponent<IdleCarTargetController>().GetData());
            return;
        }
        if (other.CompareTag("Player"))
        {
            _manager.PlayerOnRoad();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _manager.Move();
            _manager.MoveAfterPlayer();
            
        }
    }
}
