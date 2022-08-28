using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Data.ValueObject;

public class IdleCitizenPhysicsController : MonoBehaviour
{
    #region Self Variables
    #region Private Variables
    private IdleCitizenManager _manager;
    private IdleCitizenData _data;
    #endregion
    #endregion

    private void Awake()
    {
        _manager = transform.parent.GetComponent<IdleCitizenManager>();
    }
    public void GetData(IdleCitizenData data)
    {
        _data = data;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            StartCoroutine(WaitOnTargetPosition(other.GetComponent<IdleCarTargetController>().GetData()));
            return;
        }
        if (other.CompareTag("Player"))
        {

        }
    }

    IEnumerator WaitOnTargetPosition(IdleTargetData data)
    {
        yield return new WaitForSeconds(0.5f * (_data.ReachingTime / 5));
        _manager.SetAnimation(IdleCitizenAnimStates.Idle);

        yield return new WaitForSeconds(_data.WaitingTime);
        _manager.SelectRandomDirection(data);
    }
}
